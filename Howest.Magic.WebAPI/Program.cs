using FluentValidation;
using FluentValidation.AspNetCore;
using Howest.MagicCards.DAL;
using Howest.MagicCards.DAL.Repositories;
using Howest.MagicCards.Shared.DTOs;
using Howest.MagicCards.Shared.Validation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddFluentValidation(v =>
    {
        v.RegisterValidatorsFromAssemblyContaining<CardValidator>();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICardRepository, CardRepository>();

builder.Services.AddScoped<IValidator<CardDto>, CardValidator>();

builder.Services.AddAutoMapper(typeof(Howest.MagicCards.Shared.Mappings.CardProfile));

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
