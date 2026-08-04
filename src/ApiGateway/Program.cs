using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Title = "SmartTaskPlatform API Gateway";
        options.Theme = ScalarTheme.Purple;
        options.AddServer("https://localhost:7005");
        options.AddServer("http://localhost:5005");

        // YARP üzerinden IdentityService ve DocumentService OpenAPI dokümanlarını Scalar'a bağlıyoruz
        options.WithOpenApiRoutePattern("/openapi/{documentName}/v1.json");
    });
}

app.MapReverseProxy();

app.Run();