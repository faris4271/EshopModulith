using Catalog.Brands.Dtos;
using Shared.Contract.CQRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Catalog.Features.Brands.AddBrand
{
    public record CreatBrandCommand(CreatBrandDto creatBrandDto) : ICommand<Guid>;
 
}
