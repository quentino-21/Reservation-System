using Microsoft.AspNetCore.Mvc;

namespace ReservationSystem.Application.Common;

public class ErrorProblemDetails : ProblemDetails
{
    public IDictionary<string, string[]>? Errors { get; set; }
}