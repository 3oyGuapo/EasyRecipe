using Castle.Core.Logging;
using EasyRecipeAPI.Middlewares;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EasyRecipeAPI.Tests.Middlewares
{
    public class ExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_KeyNotFoundException_Returns404()
        {
            // 1. Arrange
            RequestDelegate throwingNext = (HttpContext context) =>
            {
                throw new KeyNotFoundException("Recipe not found.");
            };

            var mockLogger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();

            var middleware = new ExceptionHandlingMiddleware(throwingNext, mockLogger);

            var context = new DefaultHttpContext();

            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // 2. Act
            await middleware.InvokeAsync(context);

            // 3. Assert
            Assert.Equal(404, context.Response.StatusCode);

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var bodyContent = await new StreamReader(context.Response.Body).ReadToEndAsync();

            var errorResponse = JsonSerializer.Deserialize<JsonElement>(bodyContent);

            Assert.Equal(404, errorResponse.GetProperty("statusCode").GetInt32());
            Assert.Equal("Resource not found", errorResponse.GetProperty("message").ToString());
        }


        [Fact]
        public async Task InvokeAsync_GenericException_Returns500()
        {
            // 1. Arrange
            RequestDelegate throwingNext = (HttpContext context) =>
            {
                throw new InvalidOperationException("Database connection failed.");
            };

            var mockLogger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();

            var middleware = new ExceptionHandlingMiddleware(throwingNext, mockLogger);

            var context = new DefaultHttpContext();

            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            // 2. Act
            await middleware.InvokeAsync(context);

            // 3. Assert
            Assert.Equal(500, context.Response.StatusCode);

            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var bodyContent = await new StreamReader(context.Response.Body).ReadToEndAsync();

            var errorResponse = JsonSerializer.Deserialize<JsonElement>(bodyContent);

            Assert.Equal(500, errorResponse.GetProperty("statusCode").GetInt32());
            Assert.Equal("Internal server error", errorResponse.GetProperty("message").ToString());
        }


        [Fact]
        public async Task InvokeAsync_NoException_DoesNotModifyResponse()
        {
            // 1. Arrange
            RequestDelegate normalNext = (HttpContext context) =>
            {
                context.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            var mockLogger = Substitute.For<ILogger<ExceptionHandlingMiddleware>>();

            var middleware = new ExceptionHandlingMiddleware(normalNext, mockLogger);

            var context = new DefaultHttpContext();

            // 2. Act
            await middleware.InvokeAsync(context);

            // 3. Assert
            Assert.Equal(200, context.Response.StatusCode);
        }
    }
}
