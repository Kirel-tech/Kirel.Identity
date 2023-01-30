using Example.API.Models;
using Kirel.Identity.Core.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Example.API.Validators;

/// <inheritdoc />
public class ExUserRegistrationDtoValidator : KirelUserRegistrationDtoValidator<Guid, ExUser>
{
    /// <inheritdoc />
    public ExUserRegistrationDtoValidator(IOptions<IdentityOptions> identityOptions, UserManager<ExUser> userManager) : base(identityOptions, userManager)
    {
    }
}