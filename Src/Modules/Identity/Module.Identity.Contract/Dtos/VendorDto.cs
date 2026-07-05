namespace Module.Identity.Contract.Dtos
{
    public record VendorDto(
    Guid Id,
    string Name,
     string Slug,
     string Email,

     string Description,

     bool IsActive,

     IList<VendorManager> Managers = null);


}
