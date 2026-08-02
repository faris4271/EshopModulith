namespace Eshop.Module.Basket.Contract.Dtos
{
    public record CartRuleUsageDto(Guid Id,
                    Guid CartRuleId,
                    string CartRuleName,
                    Guid UserId,
                    string UserFullName,
                    string CouponCode,
                    Guid OrderId,
                    DateTimeOffset CreatedOn);

}
