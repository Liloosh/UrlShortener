using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server.AppExtensions;
using Server.Data;
using Server.Repositories;
using Server.Repositories.IRepositories;
using Server.Requirements;
using Server.Services;
using Server.Services.IServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.SerilogConfiguration();

//Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddSingleton<IAuthorizationHandler, UrlReqirementHandler>();

//Repositories
builder.Services.AddScoped<IUrlRepository, UrlRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRedisCachingService();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "Urls";
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddMemoryCache();

//Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(connectionString);
});

//Add Identity Core
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    //Locked out configuration
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 3;

}).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

//Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

//Add Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UrlRequirements", policy =>
    {
        policy.Requirements.Add(new UrlRequirements());
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("Client", policy =>
    {
        policy.WithOrigins("http://localhost:5181").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("Client");
app.MapControllers();

app.Run();
