
using cryptocurrency_manager.Services;
using DataContext.Context;
using DataContext.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Connection String
builder.Services.AddDbContext<CryptoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CryptoManagerContext")));

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddHostedService<PriceUpdateService>();
//builder.Services.AddScoped<IHistoryService, HistoryService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<ITradeService, TradeService>();


// Add JWT Authentication
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero
        };
    });



// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));


// Adding Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new string[] { }
    }});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CryptoDbContext>();

    // Ensure the database is created
    dbContext.Database.EnsureCreated();

    // Check if cryptocurrencies already exist to avoid duplicates
    if (!dbContext.Cryptocurrencies.Any())
    {
        var cryptocurrencies = new List<Cryptocurrency>
        {
            new Cryptocurrency { Name = "Bitcoin", Symbol = "BTC", Price = 84000M, Amount = 30000M, IsDeleted = false },
            new Cryptocurrency { Name = "Ethereum", Symbol = "ETH", Price = 4200M, Amount = 50000M, IsDeleted = false },
            new Cryptocurrency { Name = "Binance Coin", Symbol = "BNB", Price = 600M, Amount = 100000M, IsDeleted = false },
            new Cryptocurrency { Name = "Solana", Symbol = "SOL", Price = 200M, Amount = 200000M, IsDeleted = false },
            new Cryptocurrency { Name = "Cardano", Symbol = "ADA", Price = 0.5M, Amount = 5000000M, IsDeleted = false },
            new Cryptocurrency { Name = "XRP", Symbol = "XRP", Price = 1.2M, Amount = 4000000M, IsDeleted = false },
            new Cryptocurrency { Name = "Polkadot", Symbol = "DOT", Price = 8M, Amount = 300000M, IsDeleted = false },
            new Cryptocurrency { Name = "Dogecoin", Symbol = "DOGE", Price = 0.15M, Amount = 10000000M, IsDeleted = false },
            new Cryptocurrency { Name = "Avalanche", Symbol = "AVAX", Price = 50M, Amount = 200000M, IsDeleted = false },
            new Cryptocurrency { Name = "Shiba Inu", Symbol = "SHIB", Price = 0.00003M, Amount = 1000000000M, IsDeleted = false },
            new Cryptocurrency { Name = "Polygon", Symbol = "MATIC", Price = 0.7M, Amount = 3000000M, IsDeleted = false },
            new Cryptocurrency { Name = "Chainlink", Symbol = "LINK", Price = 15M, Amount = 500000M, IsDeleted = false },
            new Cryptocurrency { Name = "Cosmos", Symbol = "ATOM", Price = 10M, Amount = 400000M, IsDeleted = false },
            new Cryptocurrency { Name = "NEAR Protocol", Symbol = "NEAR", Price = 6M, Amount = 600000M, IsDeleted = false },
            new Cryptocurrency { Name = "Algorand", Symbol = "ALGO", Price = 0.2M, Amount = 8000000M, IsDeleted = false }
        };

        dbContext.Cryptocurrencies.AddRange(cryptocurrencies);
        dbContext.SaveChanges();
    }
}


app.Run();