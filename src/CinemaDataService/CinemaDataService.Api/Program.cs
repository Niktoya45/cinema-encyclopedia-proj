
using MongoDB.Driver;
using CinemaDataService.Infrastructure.Context;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;

namespace CinemaDataService.Api
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

            builder.Services.AddScoped<IMongoDatabase>(service =>
            {
                MongoUrl url = new MongoUrl(builder.Configuration.GetConnectionString("mongodb"));

                IMongoClient client = new MongoClient(url);
                IMongoDatabase db = client.GetDatabase(url.DatabaseName);

                return db;
            });
            builder.Services.AddScoped<CinemaDataDb>();

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
