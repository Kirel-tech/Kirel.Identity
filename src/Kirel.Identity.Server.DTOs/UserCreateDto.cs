using System;
using Kirel.Identity.DTOs;

namespace Kirel.Identity.Server.DTOs;

/// <inheritdoc />
public class UserCreateDto : KirelUserCreateDto<Guid, ClaimCreateDto>
{
}