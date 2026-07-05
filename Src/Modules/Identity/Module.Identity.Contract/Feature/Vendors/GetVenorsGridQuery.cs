using Module.Identity.Contract.Dtos;
using Shared.Contract.CQRS;
using Shared.Web.SmartTable;

namespace Module.Identity.Contract.Feature.Vendors
{
    public record GetVenorsGridQuery(SmartTableParam SmartTableParam) : IQuery<SmartTableResult<GetVendorListDto>>;

}
