using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Module.Core.Contract.Dtos
{
    internal record UpdateMediaDto(Guid Id, IFormFile File);
    
}
