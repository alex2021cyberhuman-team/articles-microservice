using Conduit.Articles.DomainLayer.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Conduit.Articles.PresentationLayer;

public class ExceptionFilter : IMiddleware
{
    public static bool OnException(
        Exception contextException,
        out IActionResult? result)
    {
        result = null;
        if (contextException is not ApplicationException)
        {
            return false;
        }

        result = contextException switch
        {
            BadRequestException badRequestException => new
                BadRequestObjectResult(
                    GetModelStateDictionary(badRequestException)),
            ForbiddenException => new ForbidResult(),
            NotFoundException => new NotFoundResult(),
            _ => throw new ArgumentOutOfRangeException(nameof(contextException),
                contextException, null)
        };

        return true;
    }

    private static ModelStateDictionary GetModelStateDictionary(
        BadRequestException badRequestException)
    {
        var modelState = new ModelStateDictionary();

        foreach (var validationResult in badRequestException.ValidationResults)
        {
            if (!string.IsNullOrWhiteSpace(validationResult.ErrorMessage))
            {
                modelState.AddModelError(validationResult.MemberNames.First(),
                    validationResult.ErrorMessage);
            }
        }

        return modelState;
    }

    public async Task InvokeAsync(
        HttpContext context,
        RequestDelegate next)
    {
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>() ?? throw new InvalidOperationException("Exception Filter is not set");
        
        var handled =
            OnException(exceptionHandlerPathFeature.Error, out var result);
        if (handled)
        {
            await result!.ExecuteResultAsync(new()
            {
                HttpContext = context, RouteData = context.GetRouteData()
            });
        }
        else
        {
            await next(context);
        }
    }
}
