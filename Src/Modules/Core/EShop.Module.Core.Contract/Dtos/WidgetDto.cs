using System;
using System.Collections.Generic;
using System.Text;

namespace EShop.Module.Core.Contract.Dtos
{
    public class WidgetDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string CreatUrl { get; set; }
    }
}
