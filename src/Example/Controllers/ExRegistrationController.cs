using Example.DTOs;
using Example.Models;
using Example.Services;
using Kirel.Identity.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

/// <inheritdoc />
[ApiController]
[Route("registration")]
public class ExRegistrationController : KirelRegistrationController<ExRegistrationService,ExUserRegistrationDto,Guid,ExUser>
{
    /// <inheritdoc />
    public ExRegistrationController(ExRegistrationService service) : base(service)
    {
    }
}