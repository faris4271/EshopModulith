using Shared.DDD;

namespace Eshop.Module.Core.Models
{
    public class District : EntityBase<Guid>, IAuditableEntity
    {
        public string Name { get; private set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public Guid StateOrProvinceId { get; private set; } = Guid.Empty;

        public StateOrProvince StateOrProvince { get; private set; } = null!;

        public DateTimeOffset CreatedOn { get; set; }
        public string? CreatedById { get; set; }
        public DateTimeOffset? LatestUpdatedOn { get; set; }
        public string? LatestUpdatedById { get; set; }



        private District() { }

        public static District Create(
            string name,
            string code,
            Guid stateOrProvinceId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(code);


            var district = new District
            {
                Id = Guid.NewGuid(),
                Name = name,
                Code = code,
                StateOrProvinceId = stateOrProvinceId,
                CreatedOn = DateTimeOffset.UtcNow
            };

            return district;
        }

        public void UpdateDetails(string name, string code, Guid stateOrProvinceId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            ArgumentException.ThrowIfNullOrWhiteSpace(code);


            Name = name;
            Code = code;
            StateOrProvinceId = stateOrProvinceId;
            LatestUpdatedOn = DateTimeOffset.UtcNow;
        }
    }
}