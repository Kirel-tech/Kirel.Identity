using System;
using Kirel.Identity.DTOs;

namespace Kirel.Identity.Server.DTOs;

/// <inheritdoc />
public class UserUpdateDto : KirelUserUpdateDto<Guid, ClaimUpdateDto>
{
}