using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kirel.Identity.Server.API.Filters;
/// <summary>
/// Custom attribute for enabling or disabling action execution based on a specified setting.
/// </summary>
public class EnabledControllerActionFilter : IActionFilter
{
    private readonly DisabledControllerTypes _disabledControllerTypes;
    /// <summary>
    /// Initializes a new instance of the <see cref="EnabledControllerActionFilter"/> class.
    /// </summary>
    /// <param name="disabledControllerTypes">The list of disabled controller types.</param>
 
    public EnabledControllerActionFilter(DisabledControllerTypes disabledControllerTypes)
    {
        _disabledControllerTypes = disabledControllerTypes;
    }
    
    /// <summary>
    /// Executed before the action is executed. If the controller is disabled, sets the action result to NotFoundResult.
    /// </summary>
    /// <param name="context">The context of action execution.</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (_disabledControllerTypes.Contains(context.Controller.GetType()))
            context.Result = new NotFoundResult();
    }

    /// <summary>
    /// Executed after the action is executed. Does nothing in this case.
    /// </summary>
    /// <param name="context">The context of action execution.</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Do nothing here
    }
}