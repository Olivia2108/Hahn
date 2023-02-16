using Bogus;
using HahnData.Dto;
using HahnData.Middleware.Constants;
using HahnData.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HahnData.Middleware.Enums.GeneralEnums;

namespace HahnUnitTest.Fixtures
{
	public static class EmployeeFixture
	{

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
				.RuleFor(c => c.DateCreated, f => f.Date.Recent(100)) 
				.RuleFor(c => c.Department, f => f.PickRandom(occupations));

			return faker.Generate(count);

		}

		public static int GiveMeANumber(List<Employee> employees)
		{
			List<long> IdList = employees.Select(person => person.Id).ToList();
			var myArray = IdList.ToArray();
			var exclude = new HashSet<long>(myArray);
			var range = Enumerable.Range(1, 100).Where(i => !exclude.Contains(i));

			var rand = new Random();
			int index = rand.Next(0, 100 - exclude.Count);
			return range.ElementAt(index);
		}
	}
}
