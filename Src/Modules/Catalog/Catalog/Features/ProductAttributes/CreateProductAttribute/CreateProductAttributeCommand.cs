using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductAttributes.CreateProductAttribute
{
    public record CreateProductAttributeCommand(CreateProductAttributeDto productAttributeDto) : ICommand<Guid>;
   
}
