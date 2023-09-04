using AutoMapper;

namespace Kirel.Identity.Server.Core.Mappers;

/// <summary>
/// Mapping profile for date time for converting to UTC
/// </summary>
public class DateTimeMapper : Profile
{
    /// <summary>
    /// DateTimeMapper constructor
    /// </summary>
    public DateTimeMapper()
    {
        CreateMap<DateTime, DateTime>()
            .ConvertUsing((s, d) => s.ToUniversalTime());
        CreateMap<DateTime?, DateTime?>()
            .ConvertUsing((s, d) => s?.ToUniversalTime());
    }
}