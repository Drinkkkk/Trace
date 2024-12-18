namespace Trace.Service.HttpClient
{

    public class QueryParameter
    {
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
        public string? search { get; set; } = "";
    }
}
