using Azure.AI.TextAnalytics;
using Azure;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using RTChat.Hubs;
using Infrastructure.Services.Interfaces;
using Infrastructure.Services;
using Infrastructure.Repositories.Interfaces;
using Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer(Environment.GetEnvironmentVariable("AzureDB_Connection")));

builder.Services.AddCors(options =>
{
   options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(Environment.GetEnvironmentVariable("Front_Url"))
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

builder.Services.AddSingleton(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var endpoint = new Uri(Environment.GetEnvironmentVariable("AzureAI_EndPoint"));
    var apiKey = Environment.GetEnvironmentVariable("AzureAI_API_KEY"); ;

    var credentials = new AzureKeyCredential(apiKey);
    return new TextAnalyticsClient(endpoint, credentials);
});

builder.Services.AddSignalR();

builder.Services.AddScoped<ITextAnalysisService, TextAnalysisService>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();


var app = builder.Build();


app.UseCors();
app.MapHub<ChatHub>("/chatHub");

app.Run();
