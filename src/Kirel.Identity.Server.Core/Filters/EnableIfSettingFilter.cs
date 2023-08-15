using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Kirel.Identity.Server.Core.Filters;

/// <summary>
/// Custom attribute for enabling or disabling action execution based on a specified setting.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class EnableIfSettingAttribute : Attribute, IActionFilter
{
    private readonly bool _isEnabled;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnableIfSettingAttribute"/> class.
    /// </summary>
    /// <param name="isEnabled">A value indicating whether action execution is enabled.</param>
    public EnableIfSettingAttribute(bool isEnabled)
    {
        _isEnabled = isEnabled;
    }

    /// <summary>
    /// Executed before the action is executed. If disabled, sets the action result to NotFoundResult.
    /// </summary>
    /// <param name="context">The context of action execution.</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!_isEnabled)
        {
            context.Result = new NotFoundResult();
        }
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