using System.Net.Http.Json;
using AutoMapper;
using Kirel.Blazor.Entities;
using Kirel.Blazor.Entities.Models;
using Kirel.DTOs;
using Kirel.Identity.Client.Blazor.Pages.DTOs;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;

namespace Kirel.Identity.Client.Blazor.Pages.Users;

/// <summary>
/// Identity user MudBlazor dialog base.
/// </summary>
/// <typeparam name="TClaimCreateDto">Type of create claim data transfer object</typeparam>
/// <typeparam name="TClaimUpdateDto">Type of update claim data transfer object</typeparam>
/// <typeparam name="TClaimDto">Type of claim data transfer object</typeparam>
/// <typeparam name="TRoleKey">Type of role key</typeparam>
/// <typeparam name="TRoleDto">Type of role data transfer object</typeparam>
/// <typeparam name="TUserKey">Type of user key</typeparam>
/// <typeparam name="TUserCreateDto">Type of user create data transfer object</typeparam>
/// <typeparam name="TUserUpdateDto">Type of user update data transfer object</typeparam>
/// <typeparam name="TUserDto">Type of user data transfer object</typeparam>
public partial class UserEntityDialog<TClaimCreateDto, TClaimUpdateDto, TClaimDto, TRoleKey, TRoleDto, TUserKey, TUserCreateDto, TUserUpdateDto, TUserDto> 
where TClaimCreateDto: KirelClaimCreateDto
where TClaimUpdateDto: KirelClaimUpdateDto
where TClaimDto : KirelClaimDto
where TRoleKey: IComparable, IComparable<TRoleKey>, IEquatable<TRoleKey>
where TRoleDto: KirelRoleDto<TRoleKey, TClaimDto>
where TUserKey: IComparable, IComparable<TUserKey>, IEquatable<TUserKey>
where TUserCreateDto :KirelUserCreateDto<TRoleKey, TClaimCreateDto>, new()
where TUserUpdateDto :KirelUserUpdateDto<TRoleKey, TClaimUpdateDto>, new()
where TUserDto : KirelUserDto<TUserKey, TRoleKey, TClaimDto>, new()
{
    /// <summary>
    /// Microsoft http client factory
    /// </summary>
    [Inject]
    protected IHttpClientFactory HttpClientFactory { get; set; } = null!;
    /// <summary>
    /// AutoMapper instance
    /// </summary>
    [Inject]
    protected IMapper Mapper { get; set; } = null!;
    /// <summary>
    /// Data transfer object for create the entity
    /// </summary>
    [Parameter]
    public TUserCreateDto? CreateDto  { get; set; }
    /// <summary>
    /// Data transfer object for update the entity
    /// </summary>
    [Parameter]
    public TUserUpdateDto? UpdateDto { get; set; }
    /// <summary>
    /// Data transfer object for get the entity
    /// </summary>
    [Parameter]
    public TUserDto? Dto { get; set; }
    /// <summary>
    /// Options for control dialog and fields entity settings
    /// </summary>
    [Parameter]
    public EntityOptions? Options { get; set; }

    /// <summary>
    /// Additional dialog content
    /// </summary>
    [Parameter] public RenderFragment? AdditionalContent { get; set; }
    /// <summary>
    /// Dialog properties section additional content
    /// </summary>
    [Parameter] public RenderFragment? PropertiesAdditionalContent { get; set; }
    /// <summary>
    /// Dialog claims and roles section additional content
    /// </summary>
    [Parameter] public RenderFragment? ClaimsAndRolesAdditionalContent { get; set; }
    /// <summary>
    /// Http client instance name for manage user entity
    /// </summary>
    [Parameter]
    public string UsersHttpClientName { get; set; } = "";
    /// <summary>
    /// Http client instance name for manage roles entity
    /// </summary>
    [Parameter]
    public string RolesHttpClientName { get; set; } = "";
    /// <summary>
    /// Relative url to Users API endpoint
    /// </summary>
    [Parameter]
    public string? UsersHttpRelativeUrl { get; set; }
    /// <summary>
    /// Relative url to Roles API endpoint
    /// </summary>
    [Parameter]
    public string? RolesHttpRelativeUrl { get; set; }
    /// <summary>
    /// Before create request event handler
    /// </summary>
    [Parameter]
    public Func<TUserCreateDto?, Task>? BeforeCreateRequest { get; set; }
    /// <summary>
    /// Before update request event handler
    /// </summary>
    [Parameter]
    public Func<TUserUpdateDto?, Task>? BeforeUpdateRequest { get; set; }
    
    private HttpClient _usersHttpClient = null!;
    private HttpClient _rolesHttpClient = null!;
    private List<UniversalClaimDto> _claims = new ();

    private TRoleDto? _foundedRole;
    private readonly Dictionary<TRoleKey, TRoleDto> _userRoles = new ();

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        _usersHttpClient = HttpClientFactory.CreateClient(UsersHttpClientName);
        _rolesHttpClient = HttpClientFactory.CreateClient(RolesHttpClientName);
        var action = EntityAction.Edit;
        if (Dto == null)
        {
            action = EntityAction.Create;
            Dto = new ();
        }
        Options ??= new EntityOptions()
        {
            Action = action,
        };
        if (Dto != null)
            _claims = Mapper.Map<List<UniversalClaimDto>>(Dto.Claims);
        await LoadUserRolesFormServer();
        await base.OnInitializedAsync();
    }

    private async Task LoadUserRolesFormServer()
    {
        if (Dto == null) return;
        foreach (var roleId in Dto.Roles)
        {
            var role = await _rolesHttpClient.GetFromJsonAsync<TRoleDto>($"{RolesHttpRelativeUrl}/{roleId}");
            if (role != null)
                _userRoles.TryAdd<TRoleKey, TRoleDto>(role.Id, role);
        }
    }
    
    private void AddClaim()
    {
        _claims.Add(new UniversalClaimDto());
    }
    private void RemoveClaim(UniversalClaimDto claim)
    {
        _claims.Remove(claim);
    }

    private async Task BeforeEntityCreateRequest(TUserCreateDto createDto)
    {
        createDto.Claims = Mapper.Map<List<TClaimCreateDto>>(_claims);
        createDto.Roles = _userRoles.Values.Select(r => r.Id).ToList();
        if (BeforeCreateRequest != null)
            await BeforeCreateRequest.Invoke(createDto);
    }
    private async Task BeforeEntityUpdateRequest(TUserUpdateDto updateDto)
    {
        updateDto.Claims = Mapper.Map<List<TClaimUpdateDto>>(_claims);
        updateDto.Roles = _userRoles.Values.Select(r => r.Id).ToList();
        if (BeforeUpdateRequest != null)
            await BeforeUpdateRequest.Invoke(updateDto);
    }
    
    private async Task<IEnumerable<TRoleDto>> SearchRoles(string value, CancellationToken token)
    {
        var search = !string.IsNullOrEmpty(value) ? $"search={value}" : string.Empty;
        var paginatedListDto = await _rolesHttpClient.GetFromJsonAsync<PaginatedResult<List<TRoleDto>>>(
            $"{RolesHttpRelativeUrl}/{search}", token);
        
        return paginatedListDto is { Data: { } } ? paginatedListDto.Data : new List<TRoleDto>();
    }
    private void AddRole()
    
    {
        if (_foundedRole != null)
            _userRoles.TryAdd(_foundedRole.Id, _foundedRole);
        _foundedRole = null;
    }
}