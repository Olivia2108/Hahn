using HahnData.Models;
using HahnData.Repositories.Contracts;
using HahnData.Dto;
using HahnDomain.Middleware.Constants;
using HahnDomain.Middleware.Exceptions;
using HahnDomain.Services.Contracts; 
using Microsoft.AspNetCore.Mvc;

namespace HahnAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmployeeController : Controller 
	{
		private readonly IEmployeeService _employeeService;  

		public EmployeeController(IEmployeeService employeeService)
		{
			_employeeService = employeeService;  
		}



		[HttpPost("AddEmployee")]

		[ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
		public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto employee)
		{
			try
			{
				 
				var result = await _employeeService.AddEmployees(employee);
				return result.Success ? Ok(result) : BadRequest(result);

			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
				return BadRequest(new ApiResponseDto { Message = ErrorConstants.Error });
			}
		}


		[HttpGet("GetAllEmployee")]
		[ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllEmployee()
		{
			try
			{
				var result = await _employeeService.GetEmployees();
				return result.Success ? Ok(result.Data) : BadRequest(result);

			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
				return BadRequest(new ApiResponseDto { Message = ErrorConstants.Error}) ;
			}
		}



		[HttpGet("GetEmployeeById")]
		[ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetEmployeeById([FromQuery] long employeeId)
		{
			try
			{
				
				switch (!ModelState.IsValid)
				{
					case true:
						return BadRequest(new ApiResponseDto
						{
							Message = "",
							Success = false
						}); 

				}
				 

				var result = await _employeeService.GetEmployeeById(employeeId);
				return result.Success ? Ok(result) : BadRequest(result);

			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
				return BadRequest(new ApiResponseDto { Message = ErrorConstants.Error });
			}
		}




		[HttpPut("UpdateEmployeeById")]
		[ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
		public async Task<IActionResult> UpdateEmployeeById([FromBody] EmployeeDto employee, [FromQuery] long employeeId)
		{
			try
			{
				var result = await _employeeService.UpdateEmployees(employee, employeeId);
				return result.Success ? Ok(result) : BadRequest(result);

			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
				return BadRequest(new ApiResponseDto { Message = ErrorConstants.Error });
			}
		}



		[HttpDelete("DeleteEmployeeById")]
		[ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
		public async Task<IActionResult> DeleteEmployeeById([FromQuery] long employeeId, [FromQuery] string ipAddress)
		{
			try
			{
				var result = await _employeeService.DeleteEmployeeById(employeeId, ipAddress);
				return result.Success ? Ok(result) : BadRequest(result);

			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError($"{ex.Message}  : with stack trace......  {ex.StackTrace}");
				return BadRequest(new ApiResponseDto { Message = ErrorConstants.Error });
			}
		}
	}
}
