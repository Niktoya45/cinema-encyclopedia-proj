using CinemaDataService.Api.Middlewares;
using CinemaDataService.Infrastructure.Context;
using CinemaDataService.Infrastructure.Repositories.Abstractions;
using CinemaDataService.Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Reflection;

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

            builder.Services.AddAuthentication(options =>
            {

                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = builder.Configuration.GetValue<string>("jwt:issuer");
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
