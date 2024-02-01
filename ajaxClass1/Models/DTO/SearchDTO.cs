namespace ajaxClass1.Models.DTO
{
    public class SearchDTO
    {
        public string? Keyword { get; set; }
        public int? CategoryId { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? SortBy { get; set; }
        public string? SortType { get; set; }
    }
}
