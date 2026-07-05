namespace Module.Identity.Contract.Dtos
{
    public record GetVendorListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public string Slug { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
