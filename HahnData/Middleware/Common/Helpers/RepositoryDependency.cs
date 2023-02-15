using AutoMapper;
using HahnData.Repositories.Contracts;
using HahnData.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnData.Common.Helpers
{

	public static class RepositoryDependency
	{
		public static IServiceCollection AddRepositoryDependency(this IServiceCollection services, IConfiguration? configuration)
		{


			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.Scan(i =>
			{
				i.FromCallingAssembly().AddClasses(x => x.Where(p => p.IsClass && p.Name.EndsWith("Repository")))
					.AsImplementedInterfaces()
					.WithScopedLifetime();
			});
			services.Scan(i =>
			{
				i.FromCallingAssembly().AddClasses(x => x.Where(p => p.IsClass && p.Name.EndsWith("Context")))
					.AsImplementedInterfaces()
					.WithScopedLifetime();
			});

			services.AddAutoMapper(typeof(RepositoryDependency));
			return services;


		}
	}
}
