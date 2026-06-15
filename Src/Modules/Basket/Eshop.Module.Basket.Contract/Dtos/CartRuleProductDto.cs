namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CartRuleProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsPublished { get; set; }
    }
}