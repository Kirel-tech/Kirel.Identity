using Kirel.Identity.DTOs;

namespace Example.API.DTOs;

/// <inheritdoc />
public class ExUserCreateDto : KirelUserCreateDto<Guid, KirelClaimCreateDto>
{
    
}