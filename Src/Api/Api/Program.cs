using Carter;
using Catalog;
using Eshop.Module.Core;
using FSH.Framework.Storage;
using IdentityModule;
using IdentityModule.Data;
using Microsoft.OpenApi;
using Shared.Caching;
using Shared.Extensions;
using Shared.Jobs;
using Shared.Mailing;
using Shared.Message.Extensions;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

// Increase form value count to allow large collections in form data
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue; // default is 1024
});

builder.Services.AddCarter(configurator: config =>
{
    var catalogModule = typeof(CatalogModule).Assembly.GetTypes()
        .Where(x => x.IsAssignableTo(typeof(ICarterModule))).ToArray();

    var identityModuleTypes = typeof(IdentityModule.IdentityModule).Assembly.GetTypes()
        .Where(x => x.IsAssignableTo(typeof(ICarterModule))).ToArray();

    config.WithModules(catalogModule);
    config.WithModules(identityModuleTypes);
});

var catalogAssembly = typeof(CatalogModule).Assembly;
var identityAssembly = typeof(IdentityDbContext).Assembly;

builder.Services
    .AddMediatRWithAssemblies(catalogAssembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Your Angular URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddCatalog(builder.Configuration)
    .AddCore(builder.Configuration)
    .AddIdentityModule(builder.Configuration)
    .AddMailing()
    .AddCaching(builder.Configuration)
    .AddHeroJobs();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    options.RoutePrefix = string.Empty;
});

app.UseCatalog().UseCore();
app.UseRouting();

app.UseCors("AllowAngularApp");

app.MapCarter();
app.Run();
