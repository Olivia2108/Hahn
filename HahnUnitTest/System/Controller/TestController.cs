using FluentAssertions;
using HahnAPI.Controllers;
using HahnData.Dto;
using HahnData.Models;
using HahnData.Repositories.Contracts;
using HahnDomain.Services;
using HahnDomain.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnUnitTest.System.Controller
{
	public class TestController
	{
		[Theory]
		[InlineData(3)]
		public async Task Get_OnSuccess_ReturnsStatusCode200(long id)
		{
			//Arrange

			var mockService = new Mock<IEmployeeService>();
			var systemUnderTest = new EmployeeController(mockService.Object);

			//Act 
			var result = (OkObjectResult) await systemUnderTest.GetEmployeeById(id);

			//Assert
			result.StatusCode.Should().Be(200);

		}

		[Fact]
		public async Task Get_OnSuccess_InvokesEmployeeService()
		{ 
			//Arrange
			var mockService = new Mock<IEmployeeService>();
			var mockRepo = new Mock<IEmployeeRepository>(); 
			mockService
				
				.Setup(service => service.GetEmployees())
				.Returns(Task.FromResult(new ApiResponseDto()));


			var asss = new EmployeeService(mockRepo.Object);
			var systemUnderTest = new EmployeeController(mockService.Object);

			//Act

			var result = (OkObjectResult)await systemUnderTest.GetAllEmployee();

			//Assert
			mockService.Verify(
				service =>  service.GetEmployees(), 
				Times.Once()
				);

			result.StatusCode.Should().Be(200);

		}

		[Fact]
		public async Task Get_OnSucessReturnsListsofEmployees()
		{
			//Arrange
			var mockService = new Mock<IEmployeeService>();
			var mockRepo = new Mock<IEmployeeRepository>();
			mockService

				.Setup(service => service.GetEmployees())
				.Returns(Task.FromResult(new ApiResponseDto()));


			var asss = new EmployeeService(mockRepo.Object);

			var systemUnderTest = new EmployeeController(mockService.Object);


			var result = (OkObjectResult)await systemUnderTest.GetAllEmployee();

			//Assert

			result.Should().BeOfType<OkObjectResult>();
			var objectResult = (OkObjectResult)result;

			objectResult.Value.Should().BeOfType<List<Employee>>();


			mockService.Verify(
				service => service.GetEmployees(),
				Times.Once()
				);

			result.StatusCode.Should().Be(200);
		}

		[Fact]
		public async Task Get_OnNoEmployeesBadRequestReturns400()
		{
			//Arrange
			var mockService = new Mock<IEmployeeService>();
			var mockRepo = new Mock<IEmployeeRepository>();
			mockService

				.Setup(service => service.GetEmployees())
				.Returns(Task.FromResult(new ApiResponseDto()));


			var asss = new EmployeeService(mockRepo.Object);

			var systemUnderTest = new EmployeeController(mockService.Object);


			var result = (BadRequestObjectResult)await systemUnderTest.GetAllEmployee();

			//Assert

			result.Should().BeOfType<BadRequestObjectResult>();
			var objectResult = (BadRequestObjectResult)result;

			objectResult.Value.Should().BeOfType<BadRequestObjectResult>();


			mockService.Verify(
				service => service.GetEmployees(),
				Times.Once()
				);

			result.StatusCode.Should().Be(400);
		}

		[Fact]
		public async Task Get_OnNoEmployeesFoundReturns404()
		{
			//Arrange
			var mockService = new Mock<IEmployeeService>();
			var mockRepo = new Mock<IEmployeeRepository>();
			mockService

				.Setup(service => service.GetEmployees())
				.Returns(Task.FromResult(new ApiResponseDto()));


			var asss = new EmployeeService(mockRepo.Object);

			var systemUnderTest = new EmployeeController(mockService.Object);


			var result = (NotFoundObjectResult)await systemUnderTest.GetAllEmployee();

			//Assert

			result.Should().BeOfType<NotFoundObjectResult>();
			var objectResult = (NotFoundObjectResult)result;

			objectResult.Value.Should().BeOfType<NotFoundObjectResult>();


			mockService.Verify(
				service => service.GetEmployees(),
				Times.Once()
				);

			result.StatusCode.Should().Be(404);
		}
	}
}
