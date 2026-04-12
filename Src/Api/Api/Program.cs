using Carter;
using Catalog;
using Eshop.Module.Core;
using Microsoft.OpenApi;
using Shared.Extensions;
using Shared.Message.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCarter(configurator: config =>
   {
       var catalogModule = typeof(CatalogModule).Assembly.GetTypes()
       .Where(x => x.IsAssignableTo(typeof(ICarterModule))).ToArray();
       config.WithModules(catalogModule);
   });
var catalogAssembly = typeof(CatalogModule).Assembly;
builder.Services
    .AddMediatRWithAssemblies(catalogAssembly);


builder.Services.AddCatalog(builder.Configuration).AddCore(builder.Configuration);
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
app.UseCatalog();
app.UseRouting();

app.MapCarter();
app.Run();
