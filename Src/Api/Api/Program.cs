using Carter;
using Catalog;
using Eshop.Module.Basket;
using Eshop.Module.Core;
using IdentityModule;
using IdentityModule.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi;
using Module.Inventory;
using Module.Inventory.EventHandler;
using Shared.Caching;
using Shared.Eventing;
using Shared.Extensions;
using Shared.Jobs;
using Shared.Mailing;
using Shared.Web.Auth;

var builder = WebApplication.CreateBuilder(args);

// Increase form value count to allow large collections in form data
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueCountLimit = int.MaxValue; // default is 1024
});

var catalogAssembly = typeof(CatalogModule).Assembly;
var identityAssembly = typeof(IdentityDbContext).Assembly;
var basketAssembly = typeof(BasketModule).Assembly;
var inventoryAssembly = typeof(ProductCreatedEventHandler).Assembly;


builder.Services.AddCarter(configurator: config =>
{
    var catalogModule = typeof(CatalogModule).Assembly.GetTypes()
        .Where(x => x.IsAssignableTo(typeof(ICarterModule))).ToArray();

    var identityModuleTypes = typeof(IdentityModule.IdentityModule).Assembly.GetTypes()
        .Where(x => x.IsAssignableTo(typeof(ICarterModule))).ToArray();

    var basketModuleTypes = typeof(BasketModule).Assembly.GetTypes()
        .Where(x => x.IsAssignableTo(typeof(ICarterModule))).ToArray();


    config.WithModules(catalogModule);
    config.WithModules(identityModuleTypes);
    config.WithModules(basketModuleTypes);
});

builder.Services.AddEventingCore(builder.Configuration);
builder.Services
    .AddMediatRWithAssemblies(catalogAssembly, basketAssembly, identityAssembly);
#region Cqrs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
#endregion

builder.Services.AddCatalog(builder.Configuration)
    .AddCore(builder.Configuration).
     AddBasket(builder.Configuration)
    .AddIdentityModule(builder.Configuration)
    .AddMailing()
    .AddCaching(builder.Configuration)
    .AddHeroJobs()
    .AddInventory(builder.Configuration);

//builder.Services.AddModularMassTransit(builder.Configuration, config =>
//{
//    config.AddModuleMassTransit<CatalogDbContext>(typeof(CatalogModule).Assembly);

//    //config.AddModuleMassTransit<IdentityDbContext>(typeof(IdentityDbContext).Assembly);
//    //config.AddModuleMassTransit<InventoryDbContext>(typeof(ProductCreatedConsumer).Assembly);
//});
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
//builder.Services.AddProblemDetails();
builder.Services.AddScoped<CurrentUserMiddleware>();
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
//app.UseExceptionHandler();
app.UseCatalog().UseCore().UseBasket().UseIdentity().UseInventory();
app.UseRouting();

app.UseCors("AllowAngularApp");
app.UseStaticFiles();
app.MapCarter();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<Shared.Web.Auth.CurrentUserMiddleware>();
app.Run();
