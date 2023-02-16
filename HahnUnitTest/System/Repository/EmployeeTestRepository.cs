using HahnData.DataContext;
using HahnData.Models;
using HahnData.Repositories;
using HahnData.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using FluentAssertions; 
using Xunit.Priority;
using HahnUnitTest.Helpers;

namespace HahnUnitTest.System.Repository
{
	public class EmployeeTestRepository : DbContextTextBase
	{
		 
		 
		IEmployeeRepository repository;

		[Fact] 
		public async Task Init()
		{ 
			repository = GetMock().Item1;
			var staff = GetMock().Item2;

		}


		[Fact]
		[Priority(1)]
		public async Task AddNewEmployee()
		{
			repository = GetMock().Item1;
			var result = await repository.AddEmployee(_context.Employees.FirstOrDefault());

			result.Should().NotBe(0);
		}

		[Fact]
		[Priority(2)]
		public async Task GetAllEmployee()
		{
			repository = GetMock().Item1;
			var result = await repository.GetEmployees();

			result.Count.Should().Be(10);
		}

		[Fact]
		[Priority(3)]
		public async Task GetEmployeeById()
		{
			repository = GetMock().Item1;
			//var employee = _context.Employees.FirstOrDefault();

			var employee = GetMock().Item2.FirstOrDefault();

			var result = await repository.GetEmployeeById(employee.Id);

			result.Should().NotBeNull();
		}

		[Fact]
		[Priority(4)]
		public async Task GetEmployeeById_NotExist()
		{

			repository = GetMock().Item1;
			var idNotExist = GiveMeANumber();

			var result = await repository.GetEmployeeById(idNotExist);

			result.Should().BeNull();
		}


		[Fact]
		[Priority(5)]
		public async Task UpdateEmployee()
		{

			repository = GetMock().Item1;
			var employee = _context.Employees.FirstOrDefault();

			var result = await repository.UpdateEmployeeInfo(employee.Id, employee);

			result.Should().NotBe(0);
		}

		[Fact]
		[Priority(6)]
		public async Task UpdateEmployee_NotExist()
		{

			repository = GetMock().Item1;
			var employee = _context.Employees.FirstOrDefault();
			var idNotExist = GiveMeANumber();

			var result = await repository.UpdateEmployeeInfo(idNotExist, employee);

			result.Should().NotBe(0);
		}


		[Fact]
		[Priority(7)]
		public async Task DeleteEmployee()
		{

			repository = GetMock().Item1;
			var employee = _context.Employees.FirstOrDefault();

			var result = await repository.DeleteEmployeeInfo(employee.Id, employee.IpAddress);

			result.Should().NotBe(0);
		}

		[Fact]
		[Priority(8)]
		public async Task DeleteEmployee_NotExist()
		{

			repository = GetMock().Item1;
			var employee = _context.Employees.FirstOrDefault();
			var idNotExist = GiveMeANumber();


			var result = await repository.DeleteEmployeeInfo(idNotExist, employee.IpAddress);

			result.Should().NotBe(0);
		}

		public int GiveMeANumber()
		{
			List<long> IdList = GetMock().Item2.Select(person => person.Id).ToList();
			var myArray = IdList.ToArray();
			var exclude = new HashSet<long>(myArray); 
			var range = Enumerable.Range(1, 100).Where(i => !exclude.Contains(i));

			var rand = new Random();
			int index = rand.Next(0, 100 - exclude.Count);
			return range.ElementAt(index);
		}
	}

}
