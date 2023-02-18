using FluentAssertions;
using HahnAPI.Controllers;
using HahnData.Dto;
using HahnData.Middleware.Constants;
using HahnData.Models;
using HahnData.Repositories;
using HahnDomain.Services;
using HahnUnitTest.Fixtures;
using HahnUnitTest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnUnitTest.System.Controller.EmployeeAction
{
	public class GetAction : DbContextTextBase
	{
		private readonly EmployeeRepository _repository;
		private readonly EmployeeService _employeeService;

		public GetAction()
		{
			switch (_repository)
			{
				case null:
					_repository = new EmployeeRepository(_context);
					break;
			}
			switch (_employeeService)
			{
				case null:
					_employeeService = new EmployeeService(_repository);
					break;
			}

		}

		[Fact]
		public async Task GetAllEmployees()
		{
			//Arrange   
			var systemUnderTest = new EmployeeController(_employeeService);
			 

			//Act 
			var result = (OkObjectResult)await systemUnderTest.GetAllEmployee();

			 
			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();
			result.StatusCode.Should().Be(200);

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue();
			value.Data.Should().BeOfType<List<HahnData.Models.Employee>>(); 
			value.Message.Should().BeEquivalentTo(ResponseConstants.Found);


			var json = JsonConvert.SerializeObject(value.Data);
			var employeeList = JsonConvert.DeserializeObject<List<HahnData.Models.Employee>>(json);
			employeeList.Count.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task GetAllEmployee()
		{
			//Arrange   
			var systemUnderTest = new EmployeeController(_employeeService);
			 
			//Act 
			var result = (OkObjectResult)await systemUnderTest.GetAllEmployee();


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();
			result.StatusCode.Should().Be(200);

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue();
			value.Data.Should().BeOfType<List<HahnData.Models.Employee>>();
			value.Message.Should().BeEquivalentTo(ResponseConstants.Found);


			var json = JsonConvert.SerializeObject(value.Data);
			var employeeList = JsonConvert.DeserializeObject<List<HahnData.Models.Employee>>(json);
			employeeList.Count.Should().BeGreaterThan(0); 

		}

		[Fact]
		public async Task GetEmployeeById()
		{
			//Arrange   
			var systemUnderTest = new EmployeeController(_employeeService); 
			var first = _context.Employees.FirstOrDefault();

			//Act 
			var result = (OkObjectResult)await systemUnderTest.GetEmployeeById(first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();
			result.StatusCode.Should().Be(200);

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue();
			value.Data.Should().BeOfType<HahnData.Models.Employee>();
			value.Message.Should().BeEquivalentTo(ResponseConstants.Found);
		}


		[Fact]
		public async Task GetEmployeeById_NotFound()
		{
			//Arrange   
			var systemUnderTest = new EmployeeController(_employeeService);

			var wrongId = EmployeeFixture.GiveMeANumber(_context.Employees.ToList()); 

			//Act 
			var result = (NotFoundObjectResult)await systemUnderTest.GetEmployeeById(wrongId);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();
			result.StatusCode.Should().Be(404);

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue();
			value.Data.Should().BeNull();
			value.Message.Should().BeEquivalentTo(ResponseConstants.NotFound);
		}

	}
}
