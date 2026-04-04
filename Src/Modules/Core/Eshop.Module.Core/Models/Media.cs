using Shared.DDD;
using System.ComponentModel.DataAnnotations;

namespace Eshop.Module.Core.Models
{
    public class Media : EntityBase<Guid>
    {
        public Media(int fileSize, string caption, string fileName, string mediaType)
        {
            Id = Guid.NewGuid();
            Caption = caption;
            FileSize = fileSize;
            FileName = fileName;
            MediaType = mediaType;
        }

        public int FileSize { get; private set; }

        [StringLength(450)]
        public string FileName { get; private set; }

        [StringLength(450)]
        public string Caption { get; private set; }
        public string MediaType { get; private set; }
    }
}
