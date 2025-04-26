using AutoMapper;
using cryptocurrency_manager.DataContext.Dtos;
using DataContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace cryptocurrency_manager.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Crypto mapping
            CreateMap<Cryptocurrency, CryptoDto>().ReverseMap();
            CreateMap<CryptoCreateDto, Cryptocurrency>();
            CreateMap<Cryptocurrency, CryptoHistoryDto>();
            //.ForMember(dest => dest.History, opt => opt.MapFrom(src => src.History));


            // History mapping
            CreateMap<History, HistoryDto>();

            // User mapping
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<UserUpdateDto, User>();

            //Role mapping
            CreateMap<Role, RoleDto>();

            // Wallet mapping
            CreateMap<Wallet, WalletDto>().ReverseMap();
            CreateMap<Wallet, PortfolioDto>();
            CreateMap<WalletUpdateDto, Wallet>();

            // Trade mapping
            CreateMap<Trade, TradeDto>()
                    .ForMember(dest => dest.TradeDate,
               opt => opt.MapFrom(src => src.TradeDate.ToString("yyyy-MM-dd HH:mm")))
                    .ReverseMap();
            CreateMap<Trade, TradeDetailedDto>();

            // Asset mapping
            CreateMap<Asset, AssetDto>().ReverseMap();
        }
    }
}
