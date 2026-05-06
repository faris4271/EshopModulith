using Catalog.Products.Models;
using CatalogContract.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Features.ProductOptions.CreateProductOption
{
    public record CreateProductOptionCommand(CreatProductOptionDto ProductOption) : ICommand;
   
}
