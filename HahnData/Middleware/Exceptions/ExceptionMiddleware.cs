
using HahnData.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HahnDomain.Middleware.Exceptions
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next; 
		public ExceptionMiddleware(RequestDelegate next)
		{ 
			_next = next;
		}
		public async Task Invoke(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError($"Something went wrong: {ex}");
				await HandleException(httpContext, ex);
			}
		}
		private async Task HandleException(HttpContext context, Exception exception)
		{
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
			await context.Response.WriteAsync(new ErrorInfoDto()
			{
				StatusCode = context.Response.StatusCode,
				Message = "Internal Server Error from the custom middleware."
			}.ToString());
		}
	}
}
