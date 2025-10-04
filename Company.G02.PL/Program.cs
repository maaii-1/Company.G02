using Company.G02.BLL;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Data.Contexts;
using Company.G02.DAL.Models;
using Company.G02.PL.Mapping;
using Company.G02.PL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Company.G02.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();         // Register Built-in MVC Services
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepositry>();  // Allow DI For DepartmentRepositry
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();    // Allow DI For DepartmentRepositry
            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });   // Allow DI For CompanyDbContext

            //builder.Services.AddAutoMapper(typeof(EmployeeProfile));

            builder.Services.AddAutoMapper(M =>
            {
                M.AddProfile(new EmployeeProfile());
                M.AddProfile(new DepartmentProfile());
            });
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Life Time

            //builder.Services.AddScoped();     // Lifetime: One Instance Per HTTP Request
            //builder.Services.AddTransient();  // Lifetime: New Instance Every Time Requested (Per Operation)
            //builder.Services.AddSingleton();  // Lifetime: One Instance For The Entire Application

            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddTransient<ITransientSevrvice, TransientSevrvice>();
            builder.Services.AddSingleton<ISingleton, Singleton>();

            builder.Services.AddIdentity<AppUSer, IdentityRole>()
                            .AddEntityFrameworkStores<CompanyDbContext>();

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
