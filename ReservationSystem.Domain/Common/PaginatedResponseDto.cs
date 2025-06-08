namespace ReservationSystem.Domain.Common;

public sealed record PaginatedResponseDto<T>
{
    public PaginatedResponseDto(ICollection<T> item, int pageNumber, int pageSize, int totalCount)
    {
        Items = item;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        TotalCount = totalCount;
    }
    public ICollection<T> Items { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}