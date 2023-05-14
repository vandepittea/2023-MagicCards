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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MTG API version 1",
        Version = "v1",
        Description = "API to manage cards"
    });
});

builder.Services.AddDbContext<MtgDbContext>
    (options => options.UseSqlServer(config.GetConnectionString("CardDb")));
builder.Services.AddSingleton<IMongoClient>(new MongoClient(config.GetConnectionString("MongoDb")));

builder.Services.AddAutoMapper(typeof(CardProfile));
builder.Services.AddScoped<IValidator<CardDto>, CardValidator>();
builder.Services.AddScoped<ICardRRepository, SqlCardRepository>();
builder.Services.AddScoped<ICardCUDRepository, MongoDbCardRepository>();

builder.Services.AddApiVersioning(o =>
{
    o.ReportApiVersions = true;
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
});
builder.Services.AddVersionedApiExplorer(
    options =>
    {
        // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
        // note: the specified format code will format the version as "'v'major[.minor][-status]"
        options.GroupNameFormat = "'v'VVV";

        // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
        // can also be used to control the format of the API version in route templates
        options.SubstituteApiVersionInUrl = true;
    }
);

WebApplication app = builder.Build();
string urlPrefix = config.GetSection("ApiPrefix").Value ?? commonPrefix;


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MTG API v1");
    });
}

app.UseHttpsRedirection();

app.MapCardsEndpoints(urlPrefix, app.Services.GetRequiredService<IMapper>());

app.Run();
