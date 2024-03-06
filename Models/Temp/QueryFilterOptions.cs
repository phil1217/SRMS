namespace SRMS.Models.Temp
{
    public class QueryFilterOptions
    {
        private string? _query = String.Empty;

        public string? Query { get => _query; set => _query = value?.ToLower(); }

        public PagingOptions? Page { get; set; }

    }
}
