using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using School.BLL.Interfaces;
using School.BLL.Repositores;
using School.DAL.Contexts;
using School.DAL.Models;
using School.PL.Services;
using System;

namespace School.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddDbContext<SchoolDbContext>(options =>
            {options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));});//Allow DI For AppDbContext

            builder.Services.AddScoped<IUnitOfWork,UnitOfWork>(); // Allow Dependency Injection For UnitOfWork Service
            builder.Services.AddScoped<IClassesServices,ClassesServices>(); // Allow Dependency Injection For UnitOfWork Service
            builder.Services.AddScoped<IUserServices,UserServices>(); // Allow Dependency Injection For UnitOfWork Service

            builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<SchoolDbContext>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
