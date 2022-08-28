using FluentValidation.AspNetCore;
using LAHGO.Core.Entities;
using LAHGO.Data;
using LAHGO.Service.ViewModels.CategoryVMs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LAHGO.Service.Mappings;
using LAHGO.Service.Interfaces;
using LAHGO.Service.Implementations;
using LAHGO.Core.Repositories;
using LAHGO.Data.Repositories;
using Newtonsoft.Json;

namespace LAHGO.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(options =>
            {
                options.AddProfile(new MappingProfile());
            });
            services.AddControllers()
                .AddNewtonsoftJson(options => {options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;})
                .AddFluentValidation(options => options.RegisterValidatorsFromAssemblyContaining<CategoryCreateVMVMValidator>());
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<ICategoryService, CategoryService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");

            });
        }
    }
}
