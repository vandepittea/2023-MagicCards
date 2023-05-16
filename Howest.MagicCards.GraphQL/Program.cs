using GraphQL.Server.Ui.Playground;
using Howest.MagicCards.DAL;
using Howest.MagicCards.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using GraphQL.Server;
using GraphQLAPI.GraphQLTypes;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager config = builder.Configuration;

builder.Services.AddDbContext<MtgDbContext>
    (options => options.UseSqlServer(config.GetConnectionString("CardDb")));
builder.Services.AddScoped<ICardRepository, CardRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();

builder.Services.AddScoped<RootSchema>();
builder.Services.AddGraphQL()
                .AddGraphTypes(typeof(RootSchema), ServiceLifetime.Scoped)
                .AddDataLoader()
                .AddSystemTextJson();

var app = builder.Build();

app.UseGraphQL<RootSchema>();
app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions()
{
    EditorTheme = EditorTheme.Light
}
);

app.Run();
