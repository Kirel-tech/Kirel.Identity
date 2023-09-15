namespace Kirel.Identity.DTOs;

/// <summary>
/// Pagination info 
/// </summary>
public class PaginationDto
{
    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages { get; set; }
    /// <summary>
    /// Total number of items
    /// </summary>
    public int TotalCount { get; set; }
    /// <summary>
    /// Current page number
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Size of the page
    /// </summary>
    public int Size { get; set; }
    /// <summary>
    /// Pagination constructor
    /// </summary>
    /// <param name="totalPages">Total number of pages</param>
    /// <param name="totalCount">Total number of items</param>
    /// <param name="page">Current page number</param>
    /// <param name="size">Size of the page</param>
    public PaginationDto(int totalPages = 0, int totalCount = 0, int page = 1, int size = 10)
    {
        TotalPages = totalPages;
        TotalCount = totalCount;
        Page = page;
        Size = size;
    }
    
    /// <summary>
    /// Generates pagination of entities
    /// </summary>
    /// <param name="pageNumber">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="totalCount">Total count</param>
    /// <returns>Pagination</returns>
    public static PaginationDto Generate(int pageNumber = 0, int pageSize = 0, int totalCount = 0)
    {
        var page = pageNumber > 0 ? pageNumber : 1;
        var size = pageSize > 0 ? pageSize : 10;
        var pagination = new PaginationDto()
        {
            Page = page,
            Size = size,
            TotalCount = totalCount,
            TotalPages = (int) Math.Ceiling(totalCount / (double) size)
        };
        return pagination;
    }
}
/// <summary>
/// Paginated items data
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public class PaginatedItemsDto<T>
{
    /// <summary>
    /// Pagination entity field
    /// </summary>
    public PaginationDto Pagination { get; set; }
    /// <summary>
    /// Items list
    /// </summary>
    public List<T>? Items { get; set; }
    /// <summary>
    /// PaginatedResult constructor
    /// </summary>
    public PaginatedItemsDto()
    {
        Pagination = new PaginationDto();
    }
    /// <summary>
    /// PaginatedResult constructor
    /// </summary>
    /// <param name="items"> List of paginated elements </param>
    /// <param name="page">Current page number</param>
    /// <param name="size">Size of the page</param>
    /// <param name="totalPages">Total number of pages</param>
    /// <param name="totalCount">Total number of items</param>
    public PaginatedItemsDto(List<T> items, int page, int size, int totalPages, int totalCount)
    {
        Pagination = new PaginationDto(totalPages, totalCount, page: page, size: size);
        Items = items;
    }
}