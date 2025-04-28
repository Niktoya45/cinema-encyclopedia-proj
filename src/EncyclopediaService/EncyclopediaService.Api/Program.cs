using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using EncyclopediaService.Api.Models.Utils;

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
                        opts.Conventions.AddPageRoute("/Encyclopedia/Cinemas", "{*url}");

                        opts.Conventions.AddPageRoute("/Encyclopedia/Cinemas/Cinema", "/Encyclopedia/Cinemas/{id}");
                        opts.Conventions.AddPageRoute("/Encyclopedia/Persons/Person", "/Encyclopedia/Persons/{id}");
                        opts.Conventions.AddPageRoute("/Encyclopedia/Studios/Studio", "/Encyclopedia/Studios/{id}");

                        // for test purposes
                        opts.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
                    }
                );

            builder.Services.AddSingleton<UISettings>(provider => 
            {
                UISettings settings = builder.Configuration.GetSection("ui_settings").Get<UISettings>()??throw new Exception("Missing \'ui_settings\' appsettings section or some of its elements could not be mapped");

                return settings;
            });

            builder.Services.AddTransient<BlobContainerClient>(provider =>
            {
                string uri = builder.Configuration.GetValue<string>("azure_blob:endpoint")??throw new Exception("Missing \'endpoint\' in \'azure_blob\' appsettings section");
                string acc = builder.Configuration.GetValue<string>("azure_blob:account")??throw new Exception("Missing \'account\' in \'azure_blob\' appsettings section"); 
                string key = builder.Configuration.GetValue<string>("azure_blob:key")??throw new Exception("Missing \'key\' in \'azure_blob\' appsettings section"); 
                string con = builder.Configuration.GetValue<string>("azure_blob:container")??throw new Exception("Missing \'container\' in \'azure_blob\' appsettings section");

                BlobServiceClient service = new($"DefaultEndpointsProtocol=https;AccountName={acc};AccountKey={key}");

                BlobContainerClient container = service.GetBlobContainerClient(con);

                return container;
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
