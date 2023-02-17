using FluentAssertions;
using HahnAPI.Controllers;
using HahnData.Dto;
using HahnData.Middleware.Constants;
using HahnData.Repositories;
using HahnDomain.Services;
using HahnUnitTest.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnUnitTest.System.Controller
{ 
	public class AddAction : DbContextTextBase
	{
		private EmployeeRepository _repository;
		private EmployeeService _employeeService;

		public AddAction()
		{
			if (_repository == null)
			{
				_repository = new EmployeeRepository(GetMock().Item3.Object);
			}
			if (_employeeService == null)
			{
				_employeeService = new EmployeeService(_repository);
			}

		}


		[Fact]
		public async Task AddEmployeeOnSuccess()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService); 

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault()); 
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);
			 
			//Act 
			var result = (OkObjectResult)await systemUnderTest.AddEmployee(dto);



			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeTrue();
			value.Data.Should().BeOfType<int>();
			value.Message.Should().BeEquivalentTo(ResponseConstants.Saved);


			result.StatusCode.Should().Be(200);
			 

		}


		[Fact]
		public async Task AddEmployeeOn_NoName()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Name = null;
			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);

			 
			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();	

			result.StatusCode.Should().Be(400);


		}


		[Fact]
		public async Task AddEmployeeOn_NoEmail()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Email = null;


			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}


		[Fact]
		public async Task AddEmployeeOn_InvalidEmail()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Email = dto.Email.Replace("@", "");


			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull(); 

			result.StatusCode.Should().Be(400);


		}



		[Fact]
		public async Task AddEmployeeOn_NoIpAddress()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.IpAddress = null;


			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}


		[Fact]
		public async Task AddEmployeeOn_NoPhone()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Phone = null;


			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);
			 

			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull(); 

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public async Task AddEmployeeOn_Phone_Length_Not_EqualTo_11()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Phone = dto.Phone.Remove(3);

			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);



			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse(); 
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public async Task AddEmployeeOn_NoSalary()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Salary = 0;
			 
			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public async Task AddEmployeeOn_SalaryNegative()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Salary = -10;
			 
			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}

		[Fact]
		public async Task AddEmployeeOn_NoDepartment()
		{
			//Arrange  

			var systemUnderTest = new EmployeeController(_employeeService);

			var json = JsonConvert.SerializeObject(GetMock().Item2.FirstOrDefault());
			var dto = JsonConvert.DeserializeObject<EmployeeDto>(json);

			dto.Department = null;
			 
			//Act 
			var result = (BadRequestObjectResult)await systemUnderTest.AddEmployee(dto);


			//Assert 
			result.Value.Should().BeOfType<ApiResponseDto>();

			var value = (ApiResponseDto)result.Value;
			value.Success.Should().BeFalse();
			value.Data.Should().BeNull();

			result.StatusCode.Should().Be(400);


		}


	}
}
