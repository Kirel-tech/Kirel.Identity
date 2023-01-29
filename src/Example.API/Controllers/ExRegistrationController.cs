using Example.API.DTOs;
using Example.API.Models;
using Example.API.Services;
using Kirel.Identity.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Controllers;

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