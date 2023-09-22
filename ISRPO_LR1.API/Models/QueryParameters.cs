using System.Text.Json;
using ISRPO_LR1.API.Utils;

namespace ISRPO_LR1.API.Models;

public class QueryParameters<T>
{
    private const int maxPageCount = 50;
    public int Page { get; set; } = 1;

    private int _pageCount = maxPageCount;
    public int PageCount
    {
        get { return _pageCount; }
        set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
    }

    public string? Query { get; set; } = "";

    public object Object
    {
        get
        {
            if (this.HasQuery())
                return JsonSerializer.Deserialize<T>(Query);
            return null;
        }
    }

    public string OrderBy { get; set; } = "Id";
}