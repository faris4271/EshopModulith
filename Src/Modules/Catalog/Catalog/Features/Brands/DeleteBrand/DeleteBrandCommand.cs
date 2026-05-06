using Shared.Contract.CQRS;


namespace Catalog.Features.Brands.DeleteBrand
{
    public record DeleteBrandCommand(Guid id) : ICommand<Guid>;
   
}
