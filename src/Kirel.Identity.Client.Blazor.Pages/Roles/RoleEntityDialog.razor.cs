using AutoMapper;
using Kirel.Blazor.Entities.Models;
using Kirel.Identity.Client.Blazor.Pages.DTOs;
using Kirel.Identity.DTOs;
using Microsoft.AspNetCore.Components;

namespace Kirel.Identity.Client.Blazor.Pages.Roles;

/// <summary>
/// Identity role MudBlazor dialog base.
/// </summary>
/// <typeparam name="TClaimCreateDto">Type of create claim data transfer object</typeparam>
/// <typeparam name="TClaimUpdateDto">Type of update claim data transfer object</typeparam>
/// <typeparam name="TClaimDto">Type of claim data transfer object</typeparam>
/// <typeparam name="TKey">Type of role key </typeparam>
/// <typeparam name="TCreateDto">Type of create role data transfer object</typeparam>
/// <typeparam name="TUpdateDto">Type of update role data transfer object</typeparam>
/// <typeparam name="TDto">Type of role data transfer object</typeparam>
public partial class RoleEntityDialog<TClaimCreateDto, TClaimUpdateDto, TClaimDto, TKey, TCreateDto, TUpdateDto, TDto>
    where TClaimCreateDto: KirelClaimCreateDto
    where TClaimUpdateDto: KirelClaimUpdateDto
    where TClaimDto : KirelClaimDto
    where TKey : IComparable, IComparable<TKey>, IEquatable<TKey>
    where TCreateDto : KirelRoleCreateDto<TClaimCreateDto>, new()
    where TUpdateDto : KirelRoleUpdateDto<TClaimUpdateDto>, new()
    where TDto : KirelRoleDto<TKey, TClaimDto>, new()
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
    public TCreateDto? CreateDto  { get; set; }
    /// <summary>
    /// Data transfer object for update the entity
    /// </summary>
    [Parameter]
    public TUpdateDto? UpdateDto { get; set; }
    /// <summary>
    /// Data transfer object for get the entity
    /// </summary>
    [Parameter]
    public TDto? Dto { get; set; }
    /// <summary>
    /// Options for control dialog and fields entity settings
    /// </summary>
    [Parameter]
    public EntityOptions? Options { get; set; }
    /// <summary>
    /// Before create request event handler
    /// </summary>
    [Parameter]
    public Func<TCreateDto?, Task>? BeforeCreateRequest { get; set; }
    /// <summary>
    /// Before update request event handler
    /// </summary>
    [Parameter]
    public Func<TUpdateDto?, Task>? BeforeUpdateRequest { get; set; }

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
    /// Http client instance name for manage role entity
    /// </summary>
    [Parameter]
    public string HttpClientName { get; set; } = "";
    /// <summary>
    /// Relative url to API endpoint
    /// </summary>
    [Parameter]
    public string? HttpRelativeUrl { get; set; }
    private List<UniversalClaimDto> _claims = new ();

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        var action = EntityAction.Edit;
        if (Dto == null)
        {
            action = EntityAction.Create;
        }
        Options ??= new EntityOptions()
        {
            Action = action,
        };
        if (Dto != null)
            _claims =  Mapper.Map<List<UniversalClaimDto>>(Dto.Claims);
        await base.OnInitializedAsync();
    }

    private void AddClaim()
    {
        _claims.Add(new UniversalClaimDto());
    }
    private void RemoveClaim(UniversalClaimDto claim)
    {
        _claims.Remove(claim);
    }

    private async Task BeforeEntityCreateRequest(TCreateDto createDto)
    {
        createDto.Claims = Mapper.Map<List<TClaimCreateDto>>(_claims);
        if (BeforeCreateRequest != null)
            await BeforeCreateRequest.Invoke(createDto);
    }
    private async Task BeforeEntityUpdateRequest(TUpdateDto updateDto)
    {
        updateDto.Claims = Mapper.Map<List<TClaimUpdateDto>>(_claims);
        if (BeforeUpdateRequest != null)
            await BeforeUpdateRequest.Invoke(updateDto);
    }
}