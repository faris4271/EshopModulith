using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Category.Models
{
    public class CategoryMedia:EntityBase<Guid>
    {
        public CategoryMedia(Guid categoryId,  Guid mediaId, int displayOrder)
        {
            CategoryId = categoryId;
           
            MediaId = mediaId;
            DisplayOrder = displayOrder;
        }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid MediaId { get; set; }
        public int DisplayOrder { get; set; }
    }
}
