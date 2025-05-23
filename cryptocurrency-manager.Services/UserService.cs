﻿using AutoMapper;
using cryptocurrency_manager.DataContext.Dtos;
using DataContext.Context;
using DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace cryptocurrency_manager.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(UserRegisterDto userDto);
        Task<string> LoginAsync(UserLoginDto userLoginDto);
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> UpdateUserAsync(int userid, UserUpdateDto userUpdateDto);
        Task<decimal> GetUserProfitAsync(int userId);
        Task<CryptoProfitResponse> GetUserDetailedProfitAsync(int userId);
        Task<bool> DeleteUserAsync(int id);
    }
    public class UserService : IUserService
    {

        private readonly CryptoDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UserService(CryptoDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            user.Result.IsDeleted = true;
            _context.Users.Update(user.Result);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .Include(u => u.Wallet)
                .ThenInclude(w => w.Assets)
                .Where(u => u.IsDeleted == false)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(UserLoginDto userLoginDto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Username == userLoginDto.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials.");

            return await GenerateTokenAsync(user);
        }

        private async Task<string> GenerateTokenAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var claims = await GetClaimsIdentity(user);
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims.Claims, expires: expires, signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString()) // CultureInfo.InvariantCulture (?)
            };

            if (user.Roles.Any())
            {
                claims.AddRange(user.Roles.Select(role => new Claim("roleIds", Convert.ToString(role.Id))));
                claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            }

            return new ClaimsIdentity(claims, "Token");
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Wallet = new Wallet
            {
                Balance = 1000,
                Assets = new List<Asset>()
            };
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            user.Roles = new List<Role>();

            var existingUser = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Username == userDto.Username || u.Email == userDto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("User with this username or email already exists.");
            }

            //if (userDto.RolesIds != null)
            //{
            //    foreach (var roleId in userDto.RolesIds)
            //    {
            //        var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == roleId);
            //        if (role != null)
            //        {
            //            user.Roles.Add(role);
            //        }
            //    }
            //}

            if (!user.Roles.Any())
            {
                var defaultRole = await GetDefaultRoleAsync();
                user.Roles.Add(defaultRole);
            }

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        private async Task<Role> GetDefaultRoleAsync()
        {
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (defaultRole == null)
            {
                defaultRole = new Role { Name = "User" };
                await _context.Roles.AddAsync(defaultRole);
                await _context.SaveChangesAsync();
            }
            return defaultRole;
        }
        
        public async Task<UserDto> UpdateUserAsync(int userid, UserUpdateDto userUpdateDto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userid);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }
            if (!string.IsNullOrEmpty(userUpdateDto.Name))
            {
                user.Username = userUpdateDto.Name;
            }
            if (!string.IsNullOrEmpty(userUpdateDto.Email))
            {
                user.Email = userUpdateDto.Email;
            }
            if (!string.IsNullOrEmpty(userUpdateDto.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userUpdateDto.Password);
            }
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(user);
        }

        public async Task<decimal> GetUserProfitAsync(int userId)
        {
            var wallet = await _context.Wallets
                .Where(w => w.UserId == userId)
                .Include(w => w.Assets)
                .ThenInclude(a => a.Cryptocurrency)
                .Where(w => w.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (wallet == null || wallet.Assets == null || !wallet.Assets.Any())
            {
                throw new KeyNotFoundException("No wallet or assets found for the user.");
            }

            decimal totalProfit = wallet.Assets.Sum(a =>
                (a.Cryptocurrency.Price - a.Price) * a.Amount);

            return totalProfit;
        }

        public async Task<CryptoProfitResponse> GetUserDetailedProfitAsync(int userId)
        {
            var wallet = await _context.Wallets
                .Where(w => w.UserId == userId)
                .Include(w => w.Assets)
                .ThenInclude(a => a.Cryptocurrency)
                .FirstOrDefaultAsync();

            if (wallet == null || wallet.Assets == null || !wallet.Assets.Any())
            {
                throw new KeyNotFoundException("No wallet or assets found for the user.");
            }

            var profitDetails = wallet.Assets.Select(a => new CryptoProfitDto
            {
                Symbol = a.Cryptocurrency.Symbol,
                Amount = a.Amount,
                PurchasePrice = a.Price,
                CurrentPrice = a.Cryptocurrency.Price,
                Profit = (a.Cryptocurrency.Price - a.Price) * a.Amount
            }).ToList();

            decimal totalProfit = profitDetails.Sum(d => d.Profit);

            return new CryptoProfitResponse
            {
                CryptoProfits = profitDetails,
                TotalProfit = totalProfit
            };
        }
    }
}
