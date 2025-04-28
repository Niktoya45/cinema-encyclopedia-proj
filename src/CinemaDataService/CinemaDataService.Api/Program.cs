using System.Reflection;
using MongoDB.Driver;
using CinemaDataService.Infrastructure.Context;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Infrastructure.Repositories.Implementations;

namespace CinemaDataService.Api
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

            builder.Services.AddTransient<IMongoDatabase>(service =>
            {
                MongoUrl url = new MongoUrl(builder.Configuration.GetConnectionString("mongodb"));

                IMongoClient client = new MongoClient(url);
                IMongoDatabase db = client.GetDatabase(url.DatabaseName);

                return db;
            });
            builder.Services.AddTransient<CinemaDataDb>();

            builder.Services.AddTransient<ICinemaRepository, CinemaRepository>();
            builder.Services.AddTransient<IPersonRepository, PersonRepository>();
            builder.Services.AddTransient<IStudioRepository, StudioRepository>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            builder.Services.AddAutoMapper(cfg => cfg.AddMaps(assembly));

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
