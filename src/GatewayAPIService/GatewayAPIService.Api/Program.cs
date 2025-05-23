using GatewayAPIService.Infrastructure.Services.CinemaService;
using GatewayAPIService.Infrastructure.Services.PersonService;
using GatewayAPIService.Infrastructure.Services.StudioService;
using GatewayAPIService.Infrastructure.Services.UserService;
using GatewayAPIService.Infrastructure.Services.ImageService;

namespace GatewayAPIService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string CinemaDataServiceUrl = builder.Configuration.GetConnectionString("CinemaDataService") ?? throw new Exception("Missing CinemaDataService route path in ConnectionStrings");
            string UserDataServiceUrl = builder.Configuration.GetConnectionString("UserDataService") ?? throw new Exception("Missing UserDataService route path in ConnectionStrings");
            string ImageServiceUrl = builder.Configuration.GetConnectionString("ImageService") ?? throw new Exception("Missing ImageService route path in ConnectionStrings");

            builder.Services.AddHttpClient<ICinemaService, CinemaService>(client =>
            {
                client.BaseAddress = new Uri(CinemaDataServiceUrl);
            });
            builder.Services.AddHttpClient<IPersonService, PersonService>(client =>
            {
                client.BaseAddress = new Uri(CinemaDataServiceUrl);
            });
            builder.Services.AddHttpClient<IStudioService, StudioService>(client =>
            {
                client.BaseAddress = new Uri(CinemaDataServiceUrl);
            });
            builder.Services.AddHttpClient<IUserService, UserService>(client =>
            {
                client.BaseAddress = new Uri(UserDataServiceUrl);
            });
            builder.Services.AddHttpClient<IImageService, ImageService>(client =>
            {
                client.BaseAddress = new Uri(ImageServiceUrl);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}