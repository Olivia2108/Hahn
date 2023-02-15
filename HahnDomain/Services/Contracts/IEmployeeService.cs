using HahnData.Models;
using HahnData.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnDomain.Services.Contracts
{
	public interface IEmployeeService
	{
		Task<ApiResponseDto> AddEmployees(EmployeeDto employee);
		Task<ApiResponseDto> UpdateEmployees(EmployeeDto employee, long Id);
		Task<ApiResponseDto> GetEmployees(int pageSize, int pageNumber);
		Task<ApiResponseDto> GetEmployeeById(long employeeId);
		Task<ApiResponseDto> DeleteEmployeeById(long employeeId, string ipAddress);
	}
}
