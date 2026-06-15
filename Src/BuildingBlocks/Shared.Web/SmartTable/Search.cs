using Newtonsoft.Json.Linq;

namespace Shared.Web.SmartTable
{
    public class Search
    {
        public JObject PredicateObject { get; set; } = new JObject();

        public T? GetValue<T>(string propertyName)
        {
            if (PredicateObject == null || PredicateObject[propertyName] == null)
                return default;

            try
            {
                return PredicateObject[propertyName]!.Value<T>();
            }
            catch
            {
                return default;
            }
        }

        public bool HasFilter(string propertyName)
        {
            return PredicateObject != null && PredicateObject[propertyName] != null;
        }
    }
}