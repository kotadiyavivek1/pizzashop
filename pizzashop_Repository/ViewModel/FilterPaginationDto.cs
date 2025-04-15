using System.Dynamic;

namespace pizzashop_Repository.ViewModel;

public class FilterPaginationDto<T>
{
    public List<T> Items { get; set; } = new();
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public T? Item{ get; set; }
    public string? SearchString { get; set; }
    public string? SortOrder{get; set;}
    public void SetPaginationData(List<T> items, int totalItems, int pageNumber, int pageSize, string? searchString = null)
    {
        Items = items;
        TotalItems = totalItems;
        PageNumber = pageNumber;
        PageSize = pageSize;
        SearchString = searchString;
        TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}
