using Bowling.Domain.Game.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace BowlingPainelOnBlazor.Utils
{
    public static class StartupUtils
    {
        public static void AddAllApplicationOptions(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<BusConfiguration>(Configuration.GetSection(nameof(BusConfiguration)));
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Bowling API",
                    Description = "The Bowling Panel On Blazor is an API for displaying points from a bowling game. It was developed in the C# language and aims to demonstrate the developer's knowledge of this language.",
                    Contact = new OpenApiContact
                    {
                        Name = "Ervin Notari Junior",
                        Email = "ervinnotari@hotmail.com",
                        Url = new Uri("https://www.linkedin.com/in/ervinnotarijunior/"),
                    },
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static void UseProblemDetailsExceptionHandler(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (exceptionHandlerFeature != null)
                    {
                        var exception = exceptionHandlerFeature.Error;

                        var problemDetails = new ProblemDetails
                        {
                            Instance = context.Request.HttpContext.Request.Path,
                            Type = exception.GetType().Name,
                        };

                        if (exception is HttpListenerException httpListenerException)
                        {
                            problemDetails.Title = "The request is invalid";
                            problemDetails.Status = httpListenerException.ErrorCode;
                            problemDetails.Detail = httpListenerException.Message;
                        }
                        else
                        {
                            var logger = loggerFactory.CreateLogger("GlobalExceptionHandler");
                            logger.LogError($"Unexpected error: {exceptionHandlerFeature.Error}");
                            problemDetails.Title = exception.Message;
                            problemDetails.Status = StatusCodes.Status500InternalServerError;
                            problemDetails.Detail = exception.ToString();
                        }


                        context.Response.StatusCode = problemDetails.Status.Value;
                        context.Response.ContentType = "application/problem+json";

                        var json = JsonConvert.SerializeObject(problemDetails);
                        await context.Response.WriteAsync(json);
                    }
                });
            });
        }
    }
}
