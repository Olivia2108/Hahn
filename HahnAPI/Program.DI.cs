﻿using HahnDomain.Common.Helpers; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net.Mime;
using System.Reflection;

namespace HahnAPI
{
	public partial class Program
	{
		static readonly string HahnPolicy = "_HahnPolicy";
		private static void ConfigureDiServices(IServiceCollection services)
		{

			services.AddSwaggerGen();
			services.AddMemoryCache();


			#region Api Behaviour

			services
				.AddMvc(options =>
				{ 
					options.ReturnHttpNotAcceptable = true;
					options.EnableEndpointRouting = true;
				})
				.AddControllersAsServices()
				.AddNewtonsoftJson(setupAction =>
				{
					setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
					setupAction.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					setupAction.SerializerSettings.Converters.Add(new StringEnumConverter());
				}).AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
				}).AddXmlDataContractSerializerFormatters().AddFormatterMappings(x =>
				{
					x.GetMediaTypeMappingForFormat("application/json");
				})
				.ConfigureApiBehaviorOptions(options =>
				{
					options.InvalidModelStateResponseFactory = context =>
					{
						var result = new BadRequestObjectResult(context.ModelState);
						result.ContentTypes.Add(MediaTypeNames.Application.Json);
						//result.ContentTypes.Add(MediaTypeNames.Application.Xml);
						return result;
					};
					options.SuppressConsumesConstraintForFormFileParameters = true;
					options.SuppressInferBindingSourcesForParameters = true;
					options.SuppressModelStateInvalidFilter = false;
					options.SuppressMapClientErrors = false;
					options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
				});

			services.AddControllers(options =>
			{
				options.Filters.Add(new ProducesAttribute(MediaTypeNames.Application.Json));
				options.Filters.Add(new ConsumesAttribute(MediaTypeNames.Application.Json));

			});

			#endregion


			//#region Cors
			////Cors
			//services.AddCors(options =>
			//{
			//	options.AddPolicy(name: HahnPolicy,
			//		builder =>
			//		{
			//			builder.WithOrigins("https://localhost:44318", "https://localhost:5001")
			//				.AllowAnyHeader()
			//				.WithMethods("PUT", "DELETE", "POST", "GET");
			//		});
			//});
			//#endregion



			#region Api Version
			//API VERSION
			services.AddApiVersioning(o =>
			{
				o.ReportApiVersions = true;
				o.AssumeDefaultVersionWhenUnspecified = true;
				o.DefaultApiVersion = new ApiVersion(1, 0);
				o.ApiVersionReader = new HeaderApiVersionReader("api-version");
			});
			#endregion




			#region Swagger
			//SWAGGER 
			services.AddSwaggerGen(c =>
			{
				var securitySchema = new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				};
				c.AddSecurityDefinition("Bearer", securitySchema);
				c.ResolveConflictingActions(x => x.First());
				var securityRequirement = new OpenApiSecurityRequirement { { securitySchema, new[] { "Bearer" } } };
				c.AddSecurityRequirement(securityRequirement);
				c.DescribeAllParametersInCamelCase();

				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "Hahn Api service",
					Description = "Hahn Api",
					Contact = new OpenApiContact()
					{
						Name = "HahnTeam",
						Email = "Hahnteam@Hahn.ng"
					}
				});

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
			//services.AddSwaggerGenNewtonsoftSupport();
			#endregion

		}
	}
}
