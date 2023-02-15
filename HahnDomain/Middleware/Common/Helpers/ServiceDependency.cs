using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnDomain.Common.Helpers
{
	public static class ServiceDependency
	{
		public static IServiceCollection AddServiceDependency(this IServiceCollection services, IConfiguration? configuration)
		{
			 
			services.Scan(i =>
			{
				i.FromCallingAssembly().AddClasses(x => x.Where(p => p.IsClass && p.Name.EndsWith("Service")))
					.AsImplementedInterfaces()
					.WithScopedLifetime();
			});
			 
			services.AddAutoMapper(typeof(ServiceDependency));
			return services;


		}
	}
}
