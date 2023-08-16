using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Kirel.Identity.Server.Core.Filters
{
    /// <summary>
    /// Custom attribute for enabling or disabling action execution based on a specified setting.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class EnabledControllerAttribute : Attribute, IActionFilter, IDocumentFilter
    {
        private readonly bool _isEnabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnabledControllerAttribute"/> class.
        /// </summary>
        /// <param name="isEnabled">A value indicating whether action execution is enabled.</param>
        public EnabledControllerAttribute(bool isEnabled)
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

        /// <summary>
        /// Applies the filter to the Swagger/OpenAPI document. If disabled, removes the specified controller's paths.
        /// </summary>
        /// <param name="swaggerDoc">The Swagger document.</param>
        /// <param name="context">The filter context.</param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (!_isEnabled)
            {
                var registrationController = swaggerDoc.Paths.FirstOrDefault(p => p.Key.Contains("registration"));
                if (registrationController.Key != null)
                {
                    swaggerDoc.Paths.Remove(registrationController.Key);
                }
            }
        }
    }
}
