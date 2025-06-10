namespace ReservationSystem.Client.Common;

public sealed record PageQueryFilterDto
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}