using AutoMapper;
using Example.DTOs;
using Example.Models;
using Example.Services;
using Kirel.Identity.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

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