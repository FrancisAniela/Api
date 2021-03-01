using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using WebApi.Core.Exceptions.ExceptionLogger;
using WebApi.Errors;

namespace WebApi.Middleware
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env, IExceptionLogger exceptionLogger)
        {
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    ErrorApiResponse errorResponse = new ErrorApiResponse(StatusCodes.Status500InternalServerError, ErrorCodes.Error199UnknownError);

                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    if (exceptionHandlerPathFeature != null)
                    {
                        var exceptionType = exceptionHandlerPathFeature.Error;

                        switch (exceptionType)
                        {
                            case ArgumentException exc:
                            case InvalidDataException inv:
                            case InvalidOperationException invOp:
                                {
                                    errorResponse = new ErrorApiResponse(StatusCodes.Status400BadRequest, ErrorCodes.Error124ValidationError, exceptionType.Message);
                                    break;
                                }
                            //case ExpiredException exp:
                            //    {
                            //        errorResponse = new ErrorApiResponse(StatusCodes.Status410Gone, ErrorCodes.Error106ResourceNotFound, exceptionType.Message);
                            //        break;
                            //    }
                            case NullReferenceException exc:
                                {
                                    errorResponse = new ErrorApiResponse(StatusCodes.Status404NotFound, ErrorCodes.Error106ResourceNotFound, exceptionType.Message);
                                    break;
                                }
                            //case DuplicatedRecordException exc:
                            //    {
                            //        errorResponse = new ErrorApiResponse(StatusCodes.Status409Conflict, ErrorCodes.Error109ResourceExists, exceptionType.Message);
                            //        break;
                            //    }
                            case UnauthorizedAccessException exc:
                                {
                                    errorResponse = new ErrorApiResponse(StatusCodes.Status401Unauthorized, ErrorCodes.Error130Unauthorized, exceptionType.Message);
                                    break;
                                }
                            default :
                                errorResponse = new ErrorApiResponse(StatusCodes.Status400BadRequest, ErrorCodes.Error130Unauthorized, "Bad Request");
                                break;
                        }
                        //If is 500 and is not production environment then add internal message to the error response for debugging.
                        if (errorResponse.StatusCode == StatusCodes.Status500InternalServerError)
                        {
                            if (env.EnvironmentName.Equals("production", StringComparison.OrdinalIgnoreCase))
                                errorResponse.InternalMessage = "Internal error has occurred, please contact support for more information.";
                            else
                                errorResponse.InternalMessage = exceptionType.Message;

                            exceptionLogger.LogException(exceptionType);
                        }
                    }

                    context.Response.StatusCode = errorResponse.StatusCode;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(errorResponse.ToString());
                });
            });
        }
    }
}
