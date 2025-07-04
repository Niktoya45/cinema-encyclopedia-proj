using MongoDB.Driver;
using System.Reflection;
using UserDataService.Api.Middlewares;
using UserDataService.Infrastructure.Context;
using UserDataService.Infrastructure.Repositories.Abstractions;
using UserDataService.Infrastructure.Repositories.Implementations;

namespace UserDataService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IMongoDatabase>(service =>
            {
                MongoUrl url = new MongoUrl(builder.Configuration.GetConnectionString("mongodb"));

                IMongoClient client = new MongoClient(url);
                IMongoDatabase db = client.GetDatabase(url.DatabaseName);
                return db;
            });

            builder.Services.AddTransient<UserDataDb>();

            builder.Services.AddTransient<IUserRepository, UserRepository>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
