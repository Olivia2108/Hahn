using AutoMapper;
using FluentAssertions;
using HahnAPI.Controllers;
using HahnData.DataContext;
using HahnData.Dto;
using HahnData.Models;
using HahnData.Repositories;
using HahnData.Repositories.Contracts;
using HahnDomain.Services;
using HahnDomain.Services.Contracts;
using HahnUnitTest.Fixtures;
using HahnUnitTest.Helpers; 
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq; 

namespace HahnUnitTest.System.Controller
{
	public class TestController : DbContextTextBase
	{
		private EmployeeRepository _repository;
		private EmployeeService _employeeService; 

		public TestController() 
		{
			if (_repository == null)
			{
				_repository = new EmployeeRepository(GetMock().Item3.Object);
			}
			if (_employeeService == null)
			{
				_employeeService = new EmployeeService (_repository);
			}
			
		}

		 
		
		[Fact] 
		public async Task Test()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var employeeId = GetMock().Item2.FirstOrDefault();
			//var employeeId = _context.Employees.FirstOrDefault();
			//Act 
			var result = (OkObjectResult) await systemUnderTest.GetEmployeeById(employeeId.Id);

			//Assert
			result.StatusCode.Should().Be(200);

		}

		//[Theory]
		//[InlineData(3)]
		//public async Task Get_OnSuccess_ReturnsStatusCode200(long id)
		//{
		//	//Arrange  

		//	var mockService = new Mock<IEmployeeService>(); 

		//	var systemUnderTest = new EmployeeController(_employeeService);
		//	var employeeId = _context.Employees.FirstOrDefault();

		//	//Act 
		//	var result = (OkObjectResult) await systemUnderTest.GetEmployeeById(employeeId.Id);

		//	//Assert
		//	result.StatusCode.Should().Be(200);

		//}

		//[Fact]
		//public async Task Get_OnSuccess_InvokesEmployeeService()
		//{ 
		//	//Arrange
		//	var mockService = new Mock<IEmployeeService>(); 
		//	//mockService

		//	//	.Setup(service => service.GetEmployees())
		//	//	.Returns(Task.FromResult(new ApiResponseDto()));


		//	var systemUnderTest = new EmployeeController(_employeeService);

		//	//Act

		//	var result = (OkObjectResult)await systemUnderTest.GetAllEmployee();

		//	//Assert
		//	mockService.Verify(
		//		service =>  service.GetEmployees(), 
		//		Times.Once()
		//		);

		//	result.StatusCode.Should().Be(200);

		//}

		[Fact]
		public async Task Get_OnSucessReturnsListsofEmployees()
		{
			//Arrange
			var mockService = new Mock<IEmployeeService>();


			var systemUnderTest = new EmployeeController(_employeeService);


			var result = (OkObjectResult)await systemUnderTest.GetAllEmployee();

			//Assert

			var objectResult = (OkObjectResult)result;

			objectResult.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)objectResult.Value;
			value.Success.Should().BeTrue();
			value.Should().BeOfType<List<HahnData.Models.Employee>>();


			mockService.Verify(
				service => service.GetEmployees(),
				Times.Once()
				);

			result.StatusCode.Should().Be(200);
		}

		//[Fact]
		//public async Task Get_OnNoEmployeesBadRequestReturns400()
		//{
		//	//Arrange

		//	var mockService = new Mock<IEmployeeService>();
		//	var systemUnderTest = new EmployeeController(_employeeService);


		//	var result = (BadRequestObjectResult)await systemUnderTest.GetAllEmployee();


		//	//Assert

		//	var objectResult = (BadRequestObjectResult)result;

		//	objectResult.Value.Should().BeOfType<ApiResponseDto>();

		//	var value = (ApiResponseDto)objectResult.Value;
		//	value.Success.Should().BeFalse();
		//	value.Data.Should().BeNull();


		//	mockService.Verify(
		//		service => service.GetEmployees(),
		//		Times.Once()
		//		);

		//	result.StatusCode.Should().Be(400);
		//}

		//[Fact]
		//public async Task Get_OnNoEmployeesFoundReturns404()
		//{
		//	//Arrange
		//	var mockService = new Mock<IEmployeeService>();
		//	//var mockRepo = new Mock<IEmployeeRepository>();
		//	//mockService

		//	//	.Setup(service => service.GetEmployees()) 
		//	//	.Returns(Task.FromResult(EmployeeFixture.GetResponse()));



		//	var systemUnderTest = new EmployeeController(_employeeService);


		//	var result = (NotFoundObjectResult)await systemUnderTest.GetAllEmployee();

		//	//Assert

		//	var objectResult = (NotFoundObjectResult)result;

		//	objectResult.Value.Should().BeOfType<ApiResponseDto>();

		//	var value = (ApiResponseDto)objectResult.Value;
		//	value.Success.Should().BeFalse();
		//	value.Data.Should().BeNull();

		//	mockService.Verify(
		//		service => service.GetEmployees(),
		//		Times.Once()
		//		);

		//	result.StatusCode.Should().Be(404);
		//}
	}
}
