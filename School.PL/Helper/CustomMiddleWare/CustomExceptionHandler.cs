using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;

namespace School.PL.Helper.CustomMiddleWare
{
    public class CustomExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandler> _logger;

        public CustomExceptionHandler(RequestDelegate next, ILogger<CustomExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            // Log the exception for internal purposes
            _logger.LogError(e, "An error occurred while processing the request.");

            // Handle the exception and set the appropriate response
            HandleExceptionBasedOnType(context, e);

            // Set the content type as JSON
            context.Response.ContentType = "application/json";

        }
        private void HandleExceptionBasedOnType(HttpContext context, Exception e)
        {
            if (e is ArgumentException)
                SetExceptionResult(context, e, HttpStatusCode.BadRequest, "Bad Request. Please check your input.");
            else if (e is KeyNotFoundException)
                SetExceptionResult(context, e, HttpStatusCode.NotFound, "Resource not found.");
            else if (e is UnauthorizedAccessException)
                SetExceptionResult(context, e, HttpStatusCode.Unauthorized, "Unauthorized access.");
            else
                SetExceptionResult(context, e, HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.");
        }

        private void SetExceptionResult(HttpContext context, Exception e, HttpStatusCode statusCode, string message)
        {
            // Set the response status code
            context.Response.StatusCode = (int)statusCode;

            // Create the response object
            var response = new
            {
                StatusCode = (int)statusCode,
                Message = message,
                Detailed = e.Message // Optional, adjust for security (avoid exposing sensitive data)
            };

            // Write the response as JSON
            context.Response.WriteAsJsonAsync(response);
        }
    }
}
