using GatewayAPIService.Infrastructure.Services.CinemaService;
using GatewayAPIService.Infrastructure.Services.PersonService;
using GatewayAPIService.Infrastructure.Services.StudioService;
using GatewayAPIService.Infrastructure.Services.UserService;
using GatewayAPIService.Infrastructure.Services.ImageService;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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


            builder.Services.AddAuthentication(options =>
            {

                options.DefaultScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            })
                .AddCookie()
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    var oidc = builder.Configuration.GetSection("AuthProviders:oidc");
                    options.Authority = oidc.GetValue<string>("server");
                    options.RequireHttpsMetadata = true;

                    options.ClientId = oidc.GetValue<string>("clientId");
                    options.ClientSecret = oidc.GetValue<string>("secret");
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.Scope.Add("profile");
                    options.UsePkce = false;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.SaveTokens = true;

                    options.DisableTelemetry = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateAudience = false,
                        ValidateIssuer = false,
                        AuthenticationType = "at+jwt",
                        SaveSigninToken = true,
                        NameClaimType = "name",
                        RoleClaimType = "role"

                    };

                    options.ClaimActions.MapJsonKey("role", "role");

                    options.MapInboundClaims = false;
                    options.ProtocolValidator.RequireNonce = false;

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

                });

            builder.Services.AddAuthorization(options => {
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
                        (userRole.Value.Contains("Administrator") || userRole.Value.Contains("Superadministrator"));
                    });
                });
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                IdentityModelEventSource.ShowPII = true;
            }

            app.UseHttpsRedirection();

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
                Secure = CookieSecurePolicy.Always
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}