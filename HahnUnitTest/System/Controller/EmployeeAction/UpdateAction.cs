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
	public class UpdateAction : DbContextTextBase
	{
		private readonly EmployeeRepository _repository;
		private readonly EmployeeService _employeeService;

		public UpdateAction()
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
		public async Task Update_OnSuccess()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			//Act 
			var result = (OkObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);



			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue();
			value.Data.Should().BeOfType<int>();
			value.Message.Should().BeEquivalentTo(ResponseConstants.Updated);


			result.StatusCode.Should().Be(200);


		}

		

		[Fact]
		public async Task UpdateEmployee_NotExist()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			var idNotExist = EmployeeFixture.GiveMeANumber(_context.Employees.ToList());
			//Act 
			var result = (NotFoundObjectResult)await systemUnderTest.UpdateEmployeeById(dto, idNotExist);



			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();
			result.StatusCode.Should().Be(404);

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue();
			value.Data.Should().BeNull();
			value.Message.Should().BeEquivalentTo(ResponseConstants.NotFound);




		}



		[Fact]
		public async Task UpdateOn_NoName()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Name = null;
			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}


		[Fact]
		public async Task UpdateOn_NoEmail()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Email = null;


			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}


		[Fact]
		public async Task UpdateOn_InvalidEmail()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Email = dto.Email.Replace("@", "");


			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}



		[Fact]
		public async Task UpdateOn_NoIpAddress()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.IpAddress = null;


			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}


		[Fact]
		public async Task UpdateOn_NoPhone()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Phone = null;


			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public async Task UpdateOn_Phone_Length_Not_EqualTo_11()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Phone = dto.Phone.Remove(3);

			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);



			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public async Task UpdateOn_NoSalary()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Salary = 0;

			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public async Task UpdateOn_SalaryNegative()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Salary = -10;

			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public async Task UpdateOn_NoDepartment()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var first = _context.Employees.FirstOrDefault();
			var json = JsonConvert.SerializeObject(first);
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Department = null;

			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.UpdateEmployeeById(dto, first.Id);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}

	}
}
