namespace EShop.Module.Core.Contract.Dtos
{
    public class EntityDto
    {

        public string Slug { get; set; }


        public string Name { get; set; }

        public Guid EntityId { get; set; }


        public string EntityTypeId { get; set; }

        public EntityTypeDto EntityType { get; set; }
    }
}
