using System.Diagnostics.CodeAnalysis;

using ReportService.Infrastructure.Mongo.Repositories;
using ReportService.Infrastructure.Mongo.Settings;
using ReportService.Application.Interfaces;
using ReportService.Application.Services;
using ReportService.Infrastructure.Context;
using ReportService.Infrastructure.Kafka.Handlers;

[assembly: ExcludeFromCodeCoverage]
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoSettings"));
builder.Services.AddSingleton<IMongoContext,MongoContext>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportingService, ReportingService>();
builder.Services.AddHostedService<ReportRequestedConsumer>();
builder.Services.AddScoped<IReportRequestedHandler, ReportRequestedHandler>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
