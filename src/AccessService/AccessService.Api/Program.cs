using AccessService.Api.Extensions;
using AccessService.Domain.Profiles;
using AccessService.Infrastructure.Context;
using AccessService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace AccessService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            var connectionString = (builder.Configuration.GetConnectionString("AccessDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AccessDbContextConnection' not found."))
                .Replace("(Project)", Directory.GetCurrentDirectory().Replace("Api", "Infrastructure"))
                .Replace("(CurrentUser)", Environment.UserName); ;
                

            builder.Services.AddDbContext<AccessDbContext>(options =>
                            options.UseSqlServer(connectionString)
                            );

            builder.Services.AddDefaultIdentity<AccessProfileUser>(options =>
                        options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<AccessDbContext>()
                .AddTokenProvider(nameof(AccessService), typeof(DataProtectorTokenProvider<AccessProfileUser>));



            builder.Services.AddJsonWebKeys("keys.jwks");

            builder.Services.AddHttpClient<IUserService, UserService>(client =>
            {
                string UserDataServiceUrl = builder.Configuration.GetConnectionString("UserDataService") ?? throw new Exception("Missing UserDataService route path in ConnectionStrings");

                client.BaseAddress = new Uri(UserDataServiceUrl);
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
            .AddCookie(options =>
            {
                options.LoginPath = "/account/login";

                options.Cookie = new CookieBuilder
                {
                    SameSite = SameSiteMode.None,
                    SecurePolicy = CookieSecurePolicy.Always
                };
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = builder.Configuration.GetValue<string>("AuthProviders:jwt:issuer");
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = false,
                    AuthenticationType = "at+jwt",
                    SaveSigninToken = true,
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };

                options.MapInboundClaims = false;
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

                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireAssertion(ctx =>
                    {
                        Claim? userRole = ctx.User.Claims.FirstOrDefault(c => c.Type == "role");
                        return userRole != null &&
                        userRole.Value.Contains("dministrator");
                    });
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
            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}