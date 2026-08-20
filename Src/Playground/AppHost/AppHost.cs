var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Catalog>("catalog");

builder.AddProject<Projects.Eshop_Module_Checkout>("eshop-module-checkout");

builder.AddProject<Projects.MultiTenancyModule>("multitenancy");

builder.Build().Run();
