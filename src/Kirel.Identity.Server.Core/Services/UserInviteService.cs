using AutoMapper;
using Kirel.Identity.Core.Interfaces;
using Kirel.Identity.Core.Models;
using Kirel.Identity.Core.Services;
using Kirel.Identity.Server.Domain;
using Kirel.Identity.Server.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Kirel.Identity.Server.Core.Services;

/// <inheritdoc />
public class UserInviteService : KirelUserInviteService<Guid, User, Role, UserRole, UserClaim, RoleClaim, UserInviteDto>

{
    /// <inheritdoc />
    public UserInviteService(UserManager<User> userManager, IMapper mapper, IKirelEmailSender emailSender, IKirelSmsSender smsSender, KirelInvitesOptions invitesOptions) : base(userManager, mapper, emailSender, smsSender, invitesOptions)
    {
    }
}