using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Api.Models.Utils;
using ProfileService.Infrastructure.Services.ImageService;
using ProfileService.Infrastructure.Services.GatewayService;

namespace ProfileService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddProblemDetails();

            builder.Services.Configure<RouteOptions>(opts =>
            {
                opts.LowercaseUrls = true;
                opts.LowercaseQueryStrings = true;
                opts.AppendTrailingSlash = false;
            }
                );

            builder.Services.AddRazorPages()
                .AddRazorPagesOptions(opts =>
                {
                    opts.RootDirectory = "/Views";

                    opts.Conventions.AddPageRoute("/Profiles/Profile", "/Profiles/{id}");
                    opts.Conventions.AddPageRoute("/Profiles/Marked", "/Profiles/{id}/Marked");

                    // for test purposes
                    opts.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                }
                );

            builder.Services.AddSingleton<UISettings>(provider =>
            {
                UISettings settings = builder.Configuration.GetSection("ui_settings:common").Get<UISettings>() ?? throw new Exception("Missing \'ui_settings:common\' appsettings section or some of its elements could not be mapped");

                return settings;
            });

            builder.Services.AddHttpClient<IImageService, ImageService>(client =>
            {
                string conn = builder.Configuration.GetConnectionString("ImageService") ?? throw new Exception("Missing ImageService route path in ConnectionStrings");

                client.BaseAddress = new Uri(conn);
            });

            builder.Services.AddHttpClient<IGatewayService, GatewayService>(client =>
            {
                string conn = builder.Configuration.GetConnectionString("Gateway") ?? throw new Exception("Missing Gateway route path in ConnectionStrings");

                client.BaseAddress = new Uri(conn);
            });

            builder.Services.AddAuthentication(options =>
            {

                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
            .AddCookie(opts => {
                opts.SlidingExpiration = true;

                opts.Cookie.IsEssential = true;
                opts.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                opts.Cookie.SameSite = SameSiteMode.None;
            })
            ;

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

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                CheckConsentNeeded = ctx => false,

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

