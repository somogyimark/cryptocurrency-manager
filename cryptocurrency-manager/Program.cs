
using cryptocurrency_manager.Services;
using DataContext.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Connection String
builder.Services.AddDbContext<CryptoDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("CryptoManagerContext")));

//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICryptoService, CryptoService>();
builder.Services.AddHostedService<PriceUpdateService>();
//builder.Services.AddScoped<IHistoryService, HistoryService>();
//builder.Services.AddScoped<IWalletService, WalletService>();
//builder.Services.AddScoped<ITransactionService, TransactionService>();





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

app.Run();