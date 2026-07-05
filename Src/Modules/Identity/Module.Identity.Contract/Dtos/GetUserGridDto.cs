namespace Module.Identity.Contract.Dtos
{
    public record GetUserGridDto(string Id, string Email, string FullName, DateTimeOffset CreatedOn, List<string> Roles, List<string> Groups);

}
