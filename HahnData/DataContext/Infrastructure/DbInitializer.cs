using Bogus;
using HahnData.Models;
using HahnDomain.Middleware.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HahnData.Middleware.Enums.GeneralEnums;

namespace HahnData.DataContext.Infrastructure
{
	public static class DbInitializer
	{
		public static void Initializer(HahnContext context)
		{
			SeedEverything(context);
		}


		private static void SeedEverything(HahnContext context)
		{
			context.Database.Migrate(); 
			 
			if (context.Employees.Any())
			{
				LoggerMiddleware.LogInfo("Employee exist");
			}
			else
			{
				Seed(context);

			}

		}

		private static void Seed(HahnContext context)
		{
			var stub = GenerateData(20);

			foreach (var employee in stub)
			{
				context.Employees.AddAsync(employee);
			}
			 
			var ty = context.SaveChangesAsync().GetAwaiter().GetResult();
		}

		public static List<Employee> GenerateData(int count)
		{
			var occupations = new string[] {
				nameof(Department.Administative),
				nameof(Department.Business),
				nameof(Department.HR),
				nameof(Department.Legal),
				nameof(Department.Suport),
				nameof(Department.Technology)
			};

			var faker = new Faker<Employee>()
				.RuleFor(c => c.Name, f => f.Person.FullName)
				.RuleFor(c => c.Email, f => f.Person.Email)
				.RuleFor(c => c.Phone, f => f.Person.Phone.Substring(0, 11))
				.RuleFor(c => c.Salary, f => f.Random.Decimal())
				.RuleFor(c => c.IsActive, f => true)
				.RuleFor(c => c.IpAddress, f => f.Internet.IpAddress().ToString())
				.RuleFor(c => c.DateCreated, f => f.Date.Recent(5))
				.RuleFor(c => c.Department, f => f.PickRandom(occupations));

			return faker.Generate(count);

		}

	}

}
