using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using EncyclopediaService.Api.Models.Utils;
using EncyclopediaService.Infrastructure.Services.ImageService;
using EncyclopediaService.Api.Models.Sort;

namespace EncyclopediaService.Api
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
                        opts.Conventions.AddPageRoute("/Encyclopedia/Cinemas/All", "{*url}");

                        opts.Conventions.AddPageRoute("/Encyclopedia/Cinemas/Cinema", "/Encyclopedia/Cinemas/{id}");
                        opts.Conventions.AddPageRoute("/Encyclopedia/Persons/Person", "/Encyclopedia/Persons/{id}");
                        opts.Conventions.AddPageRoute("/Encyclopedia/Studios/Studio", "/Encyclopedia/Studios/{id}");

                        // for test purposes
                        opts.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                    }
                );

            builder.Services.AddSingleton<UISettings>(provider => 
            {
                UISettings settings = builder.Configuration.GetSection("ui_settings:common").Get<UISettings>()??throw new Exception("Missing \'ui_settings:common\' appsettings section or some of its elements could not be mapped");

                return settings;
            });

            builder.Services.AddSingleton<SortCinemas>(provider =>
            {
                SortCinemas sort = builder.Configuration.GetSection("ui_settings:sort:cinemas").Get<SortCinemas>() ?? throw new Exception("Missing \'ui_settings:sort:cinemas\' appsettings section or some of its elements could not be mapped");

                return sort;
            });

            builder.Services.AddSingleton<SortPersons>(provider =>
            {
                SortPersons sort = builder.Configuration.GetSection("ui_settings:sort:persons").Get<SortPersons>() ?? throw new Exception("Missing \'ui_settings:sort:persons\' appsettings section or some of its elements could not be mapped");

                return sort;
            });

            builder.Services.AddSingleton<SortStudios>(provider =>
            {
                SortStudios sort = builder.Configuration.GetSection("ui_settings:sort:studios").Get<SortStudios>() ?? throw new Exception("Missing \'ui_settings:sort:studios\' appsettings section or some of its elements could not be mapped");

                return sort;
            });

            builder.Services.AddHttpClient<IImageService, ImageService>(client => 
            {
                string conn = builder.Configuration.GetConnectionString("ImageService") ?? throw new Exception("Missing ImageService route path in ConnectionStrings");

                client.BaseAddress = new Uri(conn);
            });

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

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
