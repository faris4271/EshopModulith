namespace Eshop.Module.Basket.Contract.Dtos
{
    public class CartRuleGridDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTimeOffset? StartOn { get; set; }
        public DateTimeOffset? EndOn { get; set; }

        public bool IsActive { get; set; }
    }
}
