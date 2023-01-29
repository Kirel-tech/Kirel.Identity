using Example.API.Models;
using Example.API.Services;
using Example.DTOs;
using Kirel.Identity.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Controllers;

/// <inheritdoc />
[ApiController]
[Route("registration")]
public class ExRegistrationController : KirelRegistrationController<ExRegistrationService, ExUserRegistrationDto, Guid, ExUser>
{
    /// <inheritdoc />
    public ExRegistrationController(ExRegistrationService service) : base(service)
    {
    }
}