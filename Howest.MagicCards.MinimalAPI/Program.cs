using Microsoft.EntityFrameworkCore;

const string commonPrefix = "/api";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient>(new MongoClient(config.GetConnectionString("MongoDb")));
builder.Services.AddAutoMapper(typeof(CardProfile));
builder.Services.AddScoped<IValidator<CardInDeckDto>, CardDeckValidator>();
builder.Services.AddScoped<IValidator<int>, IntValidator>();
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

app.MapCardsEndpoints(urlPrefix, app.Services.GetRequiredService<IMapper>(), config);

app.Run();
