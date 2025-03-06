using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School.BLL.Interfaces;
using School.BLL.Repositores;
using School.DAL.Contexts;
using School.DAL.Models;
using School.PL.Helper.CustomMiddleWare;
using School.PL.Helper.Services;
using Serilog;
using StackExchange.Redis;
using System.Text;

namespace School.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var logFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");


            // التأكد من أن مجلد Logs موجود
            Directory.CreateDirectory(logFolderPath);

            // إنشاء اسم الملف مع التاريخ الحالي
            string logFilePath = Path.Combine(logFolderPath, $"log-date_{DateTime.Now:yyyyMMdd}.txt");
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration)
                .WriteTo.File(logFilePath).CreateLogger();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddSerilog();

            builder.Services.AddDbContext<SchoolDbContext>(options =>
            {options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));});//Allow DI For AppDbContext

            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>(); // Allow Dependency Injection For UnitOfWork Service
            builder.Services.AddScoped<IClassesServices,ClassesServices>(); // Allow Dependency Injection For Classes Service
            builder.Services.AddScoped<IUserServices,UserServices>(); // Allow Dependency Injection For Users Service
            builder.Services.AddScoped<ITokenService,TokenService>(); // Allow Dependency Injection For Token Service
            builder.Services.AddScoped<IRedisService, RedisService>(); // Allow Dependency Injection For RedisServices
            builder.Services.AddSingleton<IConnectionMultiplexer>
                (
                option => ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"])
                ); // Add Redis Connection

            builder.Services.AddIdentity<AppUser,IdentityRole>()
                            .AddEntityFrameworkStores<SchoolDbContext>()
                            .AddDefaultTokenProviders();

            //builder.Services.ConfigureApplicationCookie(X =>
            //{
            //    X.LoginPath = "/Account/SignIn";
            //    X.LogoutPath = "/Account/SignOut";
            //    X.AccessDeniedPath = "/Account/AccessDenied";
            //});

            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });
            builder.Services.AddHttpClient();
            builder.Services.AddAuthorization();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseMiddleware<CustomExceptionHandler>();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
