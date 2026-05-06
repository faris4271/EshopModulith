using Catalog.Data;
using Catalog.Products.Models;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductAttributes.CreateProductAttribute
{
    internal class CreateProductAttributeCommandHandler(IGenericeRepository<ProductAttribute,CatalogDbContext> _repository) : ICommandHandler<CreateProductAttributeCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(CreateProductAttributeCommand request, CancellationToken cancellationToken)
        {
           if(request == null)
            {
                return Result.Failure<Guid>(Error.NullValue);
            }

            var attribute = new ProductAttribute
            {
                Name = new Shared.DDD.Name(request.productAttributeDto.Name),
                GroupId = request.productAttributeDto.GroupId,
            };
            await  _repository.AddAsync(attribute, cancellationToken);
            await  _repository.SaveChangesAsync();
            return Result.Success(attribute.Id);

        }
    }
}
