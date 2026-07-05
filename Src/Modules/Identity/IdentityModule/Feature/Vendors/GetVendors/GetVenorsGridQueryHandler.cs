using IdentityModule.Data;
using IdentityModule.Domain;
using Module.Identity.Contract.Dtos;
using Module.Identity.Contract.Feature.Vendors;
using Shared.Abstraction;
using Shared.Contract.CQRS;
using Shared.Contract.ResultPattern;
using Shared.Web.SmartTable;

namespace IdentityModule.Feature.Vendors.GetVendors
{
    internal class GetVenorsGridQueryHandler(IGenericeRepository<Vendor, IdentityDbContext> _vendorRepository) : IQueryHandler<GetVenorsGridQuery, SmartTableResult<GetVendorListDto>>
    {
        public async Task<Result<SmartTableResult<GetVendorListDto>>> Handle(GetVenorsGridQuery request, CancellationToken cancellationToken)
        {

            var query = await _vendorRepository.Query();


            if (request.SmartTableParam.Search.PredicateObject != null)
            {
                dynamic search = request.SmartTableParam.Search.PredicateObject;

                if (search.Email != null)
                {
                    string email = search.Email;
                    query = query.Where(x => x.Email.email.Contains(email));
                }

                if (search.CreatedOn != null)
                {
                    if (search.CreatedOn.before != null)
                    {
                        DateTimeOffset before = search.CreatedOn.before;
                        query = query.Where(x => x.CreatedOn <= before);
                    }

                    if (search.CreatedOn.after != null)
                    {
                        DateTimeOffset after = search.CreatedOn.after;
                        query = query.Where(x => x.CreatedOn >= after);
                    }
                }
            }

            var vendors = query.ToSmartTableResult(
             request.SmartTableParam,
             x => new GetVendorListDto
             {
                 Id = x.Id,
                 Name = x.Name.name,
                 Email = x.Email.email,
                 IsActive = x.IsActive,
                 Slug = x.Slug,
                 CreatedOn = x.CreatedOn
             });



            return Result.Success(vendors);

        }
    }
}
