using Azure.Storage.Blobs;
using ImageService.Api.General;
using System.Runtime;
using System.Reflection;
using ImageService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery();

builder.Services.AddTransient<BlobContainerClient>(provider =>
{
    string uri = builder.Configuration.GetValue<string>("azure_blob:endpoint") ?? throw new Exception("Missing \'endpoint\' in \'azure_blob\' appsettings section");
    string acc = builder.Configuration.GetValue<string>("azure_blob:account") ?? throw new Exception("Missing \'account\' in \'azure_blob\' appsettings section");
    string key = builder.Configuration.GetValue<string>("azure_blob:key") ?? throw new Exception("Missing \'key\' in \'azure_blob\' appsettings section");
    string con = builder.Configuration.GetValue<string>("azure_blob:container") ?? throw new Exception("Missing \'container\' in \'azure_blob\' appsettings section");

    BlobServiceClient service = new($"DefaultEndpointsProtocol=https;AccountName={acc};AccountKey={key}");

    BlobContainerClient container = service.GetBlobContainerClient(con);

    return container;
});

builder.Services.AddTransient<IImageRepository, ImageRepository>();

builder.Services.AddSingleton<ImageSettings>(provider =>
{
    ImageSettings settings = builder.Configuration.GetSection("image_settings").Get<ImageSettings>() ?? throw new Exception("Missing \'image_settings\' appsettings section or some of its elements could not be mapped");

    return settings;
});


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.MapControllers();

app.Run();
