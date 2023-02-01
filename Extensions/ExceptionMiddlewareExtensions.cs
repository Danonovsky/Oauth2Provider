using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;

namespace Oauth2Provider.Extensions;

public static class ExceptionMiddlewareExtensions {
    public static void ConfigureExceptionHandler(this IApplicationBuilder app) {
        app.UseExceptionHandler(appError => {
            appError.Run(async context => {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if(context is not null) {
                    await context.Response.WriteAsync(new ErrorDetails() {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature?.Error.Message ?? "Internal Server Error"
                    }.ToString());
                }
            });
        });
    }
}

class ErrorDetails {
    public int StatusCode {get;set;}
    public string Message {get;set;} = "";
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}