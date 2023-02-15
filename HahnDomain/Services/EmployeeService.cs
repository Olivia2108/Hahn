using FluentValidation;
using HahnData.Middleware.Constants;
using HahnData.Models;
using HahnData.Repositories.Contracts;
using HahnData.Dto;
using HahnDomain.Middleware.Constants;
using HahnDomain.Middleware.Exceptions;
using HahnDomain.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HahnData.Middleware.Enums.GeneralEnums;
using Newtonsoft.Json.Linq;

namespace HahnDomain.Services
{
	public class EmployeeService : IEmployeeService 
	{ 
		private readonly IEmployeeRepository _employeeRepository;

		public EmployeeService(IEmployeeRepository employeeRepository)
		{ 
			this._employeeRepository = employeeRepository;
		}

		public async Task<ApiResponseDto> AddEmployees(EmployeeDto employee)
		{
			try
			{

				//validate user data
				var validateData = new EmployeeValidator();
				var valid = await validateData.ValidateAsync(employee);
				switch (valid.IsValid)
				{
					case false:
						var errors = valid.Errors.Select(l => l.ErrorMessage).ToArray();
						return new ApiResponseDto
						{
							Message = string.Join("; ", errors),
							Success = false
						};
				}

				var isExist = await _employeeRepository.IsExist(email: employee.Email);
				switch (isExist)
				{
					case true:
						return new ApiResponseDto
						{
							Message = ResponseConstants.IsExist,
							Success = false
						};
				}
				

				var data = new Employee
				{  
					Salary= employee.Salary, 
					Department = employee.Department,
					Email= employee.Email,
					Name= employee.Name,
					Phone= employee.Phone, 
					IpAddress = employee.IpAddress
				};

				var result = await _employeeRepository.AddEmployee(data);
				switch (result)
				{

					case 0:
						return new ApiResponseDto
						{
							Message = ResponseConstants.NotSaved,
							Success = false
						};
					default:
						return new ApiResponseDto
						{
							Data = result,
							Message = ResponseConstants.Saved,
							Success = true
						};
				} 
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return new ApiResponseDto
				{
					Message = ErrorConstants.ServiceSaveError,
					Success = false
				}; ;

			}
		}
		
		
		public async Task<ApiResponseDto> GetEmployees()
		{
			try
			{
				var result = await _employeeRepository.GetEmployees();
				return result.Count > 0 ?
					new ApiResponseDto
					{
						Data = result,
						Message = ResponseConstants.Found,
						Success = true, 
					}

					:
					new ApiResponseDto
					{
						Message = ResponseConstants.NotFound,
						Success = true
					};
					
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return new ApiResponseDto
				{ 
					Message = ErrorConstants.ServiceFetchError,
					Success = false
				}; ;
			}
		}

		public async Task<ApiResponseDto> GetEmployeeById(long employeeId)
		{
			try
			{
				var result = await _employeeRepository.GetEmployeeById(employeeId);
				switch (result)
				{

					case null:
						return new ApiResponseDto
						{
							Message = ResponseConstants.NotFound,
							Success = false
						};
					default:
						return new ApiResponseDto
						{
							Data = result,
							Message = ResponseConstants.Found,
							Success = true
						};
				}
				 
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return new ApiResponseDto
				{
					Message = ErrorConstants.ServiceFetchError,
					Success = false
				}; ;
			}
		}

		public async Task<ApiResponseDto> UpdateEmployees(EmployeeDto employee, long Id)
		{
			try
			{

				//validate user data
				var validateData = new EmployeeValidator();
				var valid = await validateData.ValidateAsync(employee);
				switch (valid.IsValid)
				{
					case false:
						var errors = valid.Errors.Select(l => l.ErrorMessage).ToArray();
						return new ApiResponseDto
						{
							Message = string.Join("; ", errors),
							Success = false
						};
				}



				var data = new Employee
				{
					IsActive = true,
					Salary = employee.Salary,
					DateCreated = DateTime.Now,
					Department = employee.Department,
					Email = employee.Email,
					Name = employee.Name,
					Phone = employee.Phone,
				};

				var result = await _employeeRepository.UpdateEmployeeInfo(Id, data);
				switch (result)
				{

					case 0:
						return new ApiResponseDto
						{
							Message = ResponseConstants.NotUpdated,
							Success = false
						};
					default:
						return new ApiResponseDto
						{
							Data = result,
							Message = ResponseConstants.Updated,
							Success = true
						};
				}
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return new ApiResponseDto
				{
					Message = ErrorConstants.ServiceUpdateError,
					Success = false
				};

			}
		}


		public async Task<ApiResponseDto> DeleteEmployeeById(long employeeId, string ipAddress)
		{
			try
			{
				var result = await _employeeRepository.DeleteEmployeeInfo(employeeId, ipAddress);
				switch (result)
				{
					case var delete when delete == (int)DbInfo.NoIdFound:
						return new ApiResponseDto
						{
							Message = ResponseConstants.NotDeleted,
							Success = false
						};
					case var delete when delete == (int)DbInfo.ErrorThrown:
						return new ApiResponseDto
						{
							Message = ErrorConstants.DbFetchError,
							Success = false
						};
					default:
						return new ApiResponseDto
						{
							Data = result,
							Message = ResponseConstants.Deleted,
							Success = true
						};


				}

			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return new ApiResponseDto
				{
					Message = ErrorConstants.ServiceDeleteError,
					Success = false
				}; ;
			}
		}
	}
}
