using Conduit.Articles.DomainLayer.Exceptions;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Conduit.Articles.PresentationLayer;

public class ExceptionFilter : IMiddleware
{
    private static ModelStateDictionary GetModelStateDictionary(
        InvalidRequestException exception)
    {
        var modelState = new ModelStateDictionary();

        foreach (var validationResult in exception.ValidationResults)
        {
            if (!string.IsNullOrWhiteSpace(validationResult.ErrorMessage))
            {
                modelState.AddModelError(validationResult.MemberNames.First(),
                    validationResult.ErrorMessage);
            }
        }

        return modelState;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ForbiddenException)
        {
            var result = new ForbidResult();
            await result.ExecuteResultAsync(new()
            {
                HttpContext = context,
                RouteData = context.GetRouteData()
            });
        }
        catch (InvalidRequestException exception)
        {
            var result = new BadRequestObjectResult(
                GetModelStateDictionary(exception));
            await result.ExecuteResultAsync(new()
            {
                HttpContext = context,
                RouteData = context.GetRouteData()
            });
        }
        catch (BadRequestException)
        {
            var result = new BadRequestResult();
            await result.ExecuteResultAsync(new()
            {
                HttpContext = context,
                RouteData = context.GetRouteData()
            });
        }
        catch (NotFoundException)
        {
            var result = new NotFoundResult();
            await result.ExecuteResultAsync(new()
            {
                HttpContext = context,
                RouteData = context.GetRouteData()
            });
        }
    }
}

public static class ExceptionFilterMiddleware
{
    public static IApplicationBuilder UseExceptionFilter(this IApplicationBuilder applicationBuilder)
    {
        return applicationBuilder.UseMiddleware<ExceptionFilter>();
    }
}
