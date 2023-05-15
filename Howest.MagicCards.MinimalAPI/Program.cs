using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Howest.MagicCards.DAL;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.MinimalAPI.Mappings;
using Howest.MagicCards.Shared.DTO;
using Howest.MagicCards.Shared.Mappings;
using Howest.MagicCards.Shared.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

const string commonPrefix = "/api";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(v =>
    {
        v.RegisterValidatorsFromAssemblyContaining<CardValidator>();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient>(new MongoClient(config.GetConnectionString("MongoDb")));

builder.Services.AddAutoMapper(typeof(CardProfile));
builder.Services.AddScoped<IValidator<CardDto>, CardValidator>();
builder.Services.AddScoped<IDeckRepository, DeckRepository>();

WebApplication app = builder.Build();
string urlPrefix = config.GetSection("ApiPrefix").Value ?? commonPrefix;


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapCardsEndpoints(urlPrefix, app.Services.GetRequiredService<IMapper>());

app.Run();
