using FluentAssertions;
using HahnAPI.Controllers;
using HahnData.Dto;
using HahnData.Middleware.Constants;
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

namespace HahnUnitTest.System.Controller.Employee
{
	public class DeleteAction : DbContextTextBase
	{
		private EmployeeRepository _repository;
		private EmployeeService _employeeService;

		public DeleteAction()
		{
			if (_repository == null)
			{
				_repository = new EmployeeRepository(_context);
			}
			if (_employeeService == null)
			{
				_employeeService = new EmployeeService(_repository);
			}

		}

		[Fact]
		public async Task DeleteOn_Success()
		{
			//Arrange   
			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			//Act 
			var result = (OkObjectResult)await systemUnderTest.DeleteEmployeeById(first.Id, first.IpAddress);



			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>(); 
			result.StatusCode.Should().Be(200);

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue(); 
			value.Message.Should().BeEquivalentTo(ResponseConstants.Deleted);
			 
		}


		[Fact]
		public async Task DeleteOn_Failed()
		{
			//Arrange   
			var systemUnderTest = new EmployeeController(_employeeService);

			var wrongId = EmployeeFixture.GiveMeANumber(_context.Employees.ToList());
			var first = _context.Employees.FirstOrDefault();


			//Act 
			var result = (NotFoundObjectResult)await systemUnderTest.DeleteEmployeeById(wrongId, first.IpAddress);
			 

			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();
			result.StatusCode.Should().Be(404);

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue();
			value.Message.Should().BeEquivalentTo(ResponseConstants.NotDeleted);

		}

	}
}
