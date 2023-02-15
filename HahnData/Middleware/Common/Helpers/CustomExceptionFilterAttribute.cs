using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using HahnDomain.Middleware.Exceptions;

namespace HahnDomain.Common.Helpers
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			if (context.Exception is ValidationException)
			{
				context.HttpContext.Response.ContentType = "application/json";
				context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				//context.Result = new JsonResult(
				//    ((ValidationException)context.Exception).Failures);
				context.Result = new JsonResult(new
				{
					error = new[] { context.Exception.Message },
					instance = context.HttpContext.Request.Path,
					stackTrace = context.Exception.StackTrace
				});

				return;
			}

			var code = HttpStatusCode.InternalServerError;

			if (context.Exception is NotFoundException)
			{
				code = HttpStatusCode.NotFound;
			}

			context.HttpContext.Response.ContentType = "application/json";
			context.HttpContext.Response.StatusCode = (int)code;
			context.Result = new JsonResult(new
			{
				error = new[] { context.Exception.Message },
				instance = context.HttpContext.Request.Path,
				stackTrace = context.Exception.StackTrace
			});
		}
	}
}
