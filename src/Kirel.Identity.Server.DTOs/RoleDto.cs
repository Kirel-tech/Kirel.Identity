using System;
using Kirel.Identity.DTOs;

namespace Kirel.Identity.Server.DTOs;

/// <inheritdoc />
public class RoleDto : KirelRoleDto<Guid, ClaimDto>
{
}