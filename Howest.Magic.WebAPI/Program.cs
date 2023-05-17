using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Howest.MagicCards.DAL;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MTG API version 1",
        Version = "v1",
        Description = "API to manage cards"
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "MTG API version 2",
        Version = "v2",
        Description = "API to manage cards"
    });
});

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MtgDbContext>
    (options => options.UseSqlServer(config.GetConnectionString("CardDb")));


builder.Services.AddScoped<ICardColorRepository, CardRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = config.GetConnectionString("Redis");
    options.InstanceName = "Redis_";
});

builder.Services.AddAutoMapper(typeof(CardProfile));

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MTG API v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "MTG API v2");
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
