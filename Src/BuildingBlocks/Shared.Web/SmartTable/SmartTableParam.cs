namespace Shared.Web.SmartTable
{
    public class SmartTableParam
    {
        public Pagination Pagination { get; set; } = new Pagination();

        public Search Search { get; set; } = new Search();

        public Sort Sort { get; set; } = new Sort();
    }
}
