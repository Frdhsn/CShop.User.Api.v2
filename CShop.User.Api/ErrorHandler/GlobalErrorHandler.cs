using CShop.User.Service.CustomException;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace CShop.User.Api.ErrorHandler
{
    public static class GlobalErrorHandler
    {
        public static int GetStatusCode(Type type)
        {
            if (type == typeof(BadRequestHandler)) return (int)HttpStatusCode.BadRequest;
            else if (type == typeof(UnauthorisedHandler)) return (int)HttpStatusCode.Unauthorized;
            else if (type == typeof(NotFoundHandler)) return (int)HttpStatusCode.NotFound;
            else if (type == typeof(ForbiddenHandler)) return (int)HttpStatusCode.Forbidden;
            else return (int)HttpStatusCode.InternalServerError;
        }
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    //context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        //logger.LogError($"Something went wrong: {contextFeature.Error}");
                        Type type = contextFeature.Error.GetType();
                        context.Response.StatusCode = GetStatusCode(type);
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());
                    }
                    else
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "An unknown error occured",
                        }.ToString());
                    }
                });
            });
        }
    }
}
