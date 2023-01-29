using AutoMapper;
using Example.API.Models;
using Example.API.Services;
using Example.DTOs;
using Kirel.Identity.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.API.Controllers;

/// <inheritdoc />
[ApiController]
[Authorize]
[Route("authorized/user")]
public class ExAuthorizedUserController : KirelAuthorizedUserController<ExAuthorizedUserService,Guid,ExUser,
    ExAuthorizedUserDto,ExAuthorizedUserUpdateDto>
{
    /// <inheritdoc />
    public ExAuthorizedUserController(ExAuthorizedUserService service, IMapper mapper) : base(service, mapper)
    {
    }
}