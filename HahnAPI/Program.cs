using FluentValidation.AspNetCore;
using HahnData.Common.Helpers;
using HahnData.DataContext;
using HahnData.DataContext.Infrastructure;
using HahnData.Repositories;
using HahnData.Repositories.Contracts;
using HahnDomain.Common.Helpers;
using HahnDomain.Middleware.Exceptions;
using HahnDomain.Services;
using HahnDomain.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System.Net;
using System.Net.Mime;
using System.Reflection;

namespace HahnAPI
{
	public partial class Program
	{
		private static readonly string? Namespace = typeof(Program).Namespace;
		public static readonly string? AppName = Namespace; 

		private static IConfiguration GetConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.Development.json", optional: true) 
				.AddEnvironmentVariables();

			return builder.Build();
		}
		private static IConfiguration Configuration { get; set; } 

		public static void Main(string[] args)
		{
			Configuration = GetConfiguration();
			try
			{

				var builder = WebApplication.CreateBuilder(args);

				// Add services to the container.

				builder.Services.AddControllers(); 


				var env = Configuration.GetValue<string>("Env:docker");
				string conectionStringn = string.Empty;
				switch (env)
				{
					case "true":
						conectionStringn = builder.Configuration.GetConnectionString("HahnConn_Docker");
						break;

					default:
						conectionStringn = builder.Configuration.GetConnectionString("HahnConn_Local");
						break;
				}


				builder.Services.AddDbContext<HahnContext>(options =>
				{
					options.UseSqlServer(conectionStringn, b => b.MigrationsAssembly(typeof(HahnContext).Assembly.FullName));
					//options.UseSqlServer(builder.Configuration.GetConnectionString("HahnConn"));
				});
				 

				builder.Services.AddServiceDependency(Configuration);
				builder.Services.AddRepositoryDependency(Configuration);

				//Call the Program DI class
				ConfigureDiServices(builder.Services);

				var app = builder.Build();

				//Configure Exception Middelware
				app.UseMiddleware<ExceptionMiddleware>();


				SeedDatabase();

				void SeedDatabase()
				{
					using var scope = app.Services.CreateScope();
					var scopedContext = scope.ServiceProvider.GetRequiredService<HahnContext>();
					DbInitializer.Initializer(scopedContext);
				}




				// Configure the HTTP request pipeline.
				if (app.Environment.IsDevelopment())
				{
					app.UseSwagger();
					app.UseSwaggerUI();
				}
				app.UseAuthentication();

				app.UseRouting();
				//app.UseHttpsRedirection();

				app.UseCors(options => 
					options.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowAnyOrigin()
				);


				app.UseAuthorization();

				app.MapControllers();
				 

				app.Run();


			}
			catch (Exception ex)
			{
				var type = ex.GetType().Name;
				if (type.Equals("StopTheHostException", StringComparison.Ordinal))
				{
					throw;
				}
				LoggerMiddleware.LogError($"Program terminated unexpectedly (ApplicationContext)! with appname {AppName} and Ex.Message being {ex}" ) ; 
			}
			finally
			{
				LogManager.Shutdown();
			}
		}
	}
}