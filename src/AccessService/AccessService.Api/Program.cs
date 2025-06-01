using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AccessService.Domain.Profiles;
using AccessService.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;
using AccessService.Api.Extensions;

namespace AccessService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("AccessDbContextConnection")
                ?? throw new InvalidOperationException("Connection string 'AccessDbContextConnection' not found.");

            builder.Services.AddDbContext<AccessDbContext>(options =>
                            options.UseSqlServer(connectionString)
                            );

            builder.Services.AddDefaultIdentity<AccessProfileUser>(options =>
                        options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<AccessDbContext>();

            builder.Services.AddJsonWebKeys("keys.jwks");

            builder.Services
                  .ConfigureApplicationCookie(options =>
                  {
                      options.LoginPath = "/account/login";

                      options.Cookie = new CookieBuilder
                      {
                          SameSite = SameSiteMode.Lax,
                          SecurePolicy = CookieSecurePolicy.Always
                      };
                  });

            builder.Services.Configure<RouteOptions>(opts =>
            {
                opts.LowercaseUrls = true;
                opts.LowercaseQueryStrings = false;
                opts.AppendTrailingSlash = false;
            }
            );

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Authenticated", policy =>
                {
                    policy.RequireAuthenticatedUser();
                });
            });

            builder.Services.AddControllers();

            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseHttpsRedirection();

            app.MapControllers();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}