using LAHGO.Service.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using static LAHGO.Service.Exceptions.ItemNotFoundException;
using static LAHGO.Service.Exceptions.AlreadeExistException;

namespace LAHGO.Service.Extensions
{
    public static class ExceptionHandler
    {
        public static void ExceptionHadler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    var feature = context.Features.Get<IExceptionHandlerPathFeature>();

                    int statusCode = 500;
                    string errorMessage = "Internal Server Error";

                    if (feature.Error is ItemtNoteFoundException)
                    {
                        statusCode = 404;
                        errorMessage = feature.Error.Message;
                    }
                    else if (feature.Error is AlreadeExistException)
                    {
                        statusCode = 409;
                        errorMessage = feature.Error.Message;
                    }

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsync(errorMessage);
                });
            });
        }
    }
}
