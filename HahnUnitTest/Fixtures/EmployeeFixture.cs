using HahnData.Models;
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
		public static List<Employee> GetEmployees() =>
			new()
			{
				new Employee
				{
					Name ="Test User 1",
					Email = "test.user.1@hahn.ng",
					DateCreated= DateTime.Now,
					Department = nameof(Department.Suport),
					Salary = 10000,
					IpAddress = "yyyh",
					IsActive= true,
					Phone = "08066460722",
					

				},
				new Employee
				{
					Name ="Test User 2",
					Email = "test.user.2@hahn.ng",
					DateCreated= DateTime.Now,
					Department = nameof(Department.Administative),
					Salary = 10000,
					IpAddress = "yyyh",
					IsActive= true,
					Phone = "08066460722",


				},
				new Employee
				{
					Name ="Test User 3",
					Email = "test.user.3@hahn.ng",
					DateCreated= DateTime.Now,
					Department = nameof(Department.Business),
					Salary = 10000,
					IpAddress = "yyyh",
					IsActive= true,
					Phone = "08066460722",


				},
				new Employee
				{
					Name ="Test User 4",
					Email = "test.user.4@hahn.ng",
					DateCreated= DateTime.Now,
					Department = nameof(Department.HR),
					Salary = 10000,
					IpAddress = "yyyh",
					IsActive= true,
					Phone = "08066460722",


				}

			};
	}
}
