namespace Module.Identity.Contract.Dtos
{
    public record CreatVendorDto(
        Guid Id,
    string Name,
     string Slug,
     string Email,

     string Description,

     bool IsActive);

}
