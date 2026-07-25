using BuildingBlocks.Core.Infrastructure;
using BuildingBlocks.Messaging;
using DocumentService.Application;
using DocumentService.Infrastructure;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

// Add Clean Architecture layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCustomMassTransit(builder.Configuration, typeof(Program).Assembly);

// Add Controller and API services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Shared Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();

app.Run();
