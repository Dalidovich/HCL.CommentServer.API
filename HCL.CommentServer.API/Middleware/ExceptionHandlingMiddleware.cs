﻿using HCL.CommentServer.API.Domain.DTO;
using HCL.CommentServer.API.Domain.DTO.Builders;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace HCL.CommentServer.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    (int)HttpStatusCode.NotFound,
                    "Entity not found");
            }
            catch (AuthenticationException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    (int)HttpStatusCode.Unauthorized,
                    "Authentication error");
            }
            catch (PostgresException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    (int)HttpStatusCode.ServiceUnavailable,
                    "Database service temporarily unavailable");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    (int)HttpStatusCode.InternalServerError,
                    "Internal server error");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string exMsg, int httpStatusCode, string message)
        {
            var log = new LogDTOBuidlder("HandleExceptionAsync")
                .BuildMessage($"error - {exMsg}\nmessage - {message}")
                .BuildStatusCode(httpStatusCode)
                .Build();
            _logger.LogError(JsonSerializer.Serialize(log));

            HttpResponse response = context.Response;

            response.ContentType = "application/json";
            response.StatusCode = httpStatusCode;

            ErrorDTO errorDto = new()
            {
                Message = message,
                StatusCode = httpStatusCode
            };

            await response.WriteAsJsonAsync(errorDto);
        }
    }
}