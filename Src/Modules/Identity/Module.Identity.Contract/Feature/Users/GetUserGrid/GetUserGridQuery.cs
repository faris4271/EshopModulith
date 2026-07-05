

using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;
using Shared.Web.SmartTable;

namespace Module.Identity.Contract.Feature.Users.GetUserGrid
{
    public record GetUserGridQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<GetUserGridDto>>;

}
