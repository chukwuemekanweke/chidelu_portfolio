using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebBackendBoilerPlate.Models;

namespace WebBackendBoilerPlate.Infrastructure
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        if (contextFeature.Error.GetType() == typeof(InvalidOperationException))
                        {

                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                Status = ResponseStatus.APP_ERROR,
                                Message = contextFeature.Error.Message,
                            }.ToString());

                        }
                        else if (contextFeature.Error.GetType() == typeof(Exception))
                        {
                            await context.Response.WriteAsync(new ErrorDetails
                            {
                                Status = ResponseStatus.FATAL_ERROR,
                                Message = "Oops Something Went Wrong"
                            }.ToString());
                        }
                    }
                });
            });
        }
    }

}
