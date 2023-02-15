using HahnData.DataContext;
using HahnData.DataContext.Contracts;
using HahnData.Middleware.Pagination;
using HahnData.Models;
using HahnData.Repositories.Contracts;
using HahnDomain.Middleware.Exceptions;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using NLog.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HahnData.Middleware.Enums.GeneralEnums;

namespace HahnData.Repositories
{
	public class EmployeeRepository : IEmployeeRepository
	{ 
		private readonly HahnContext _context;

		public EmployeeRepository(HahnContext context)
		{
			this._context = context;
		}





		public async Task<long> AddEmployee(Employee data)
		{
			try
			{
				await _context.Employees.AddAsync(data);
				//var saves = await _context.SaveChangesAsync(data.IpAddress);
				var saves = await _context.SaveChangesAsync();
				return data.Id;
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return (int)DbInfo.ErrorThrown;
			}
		}


		public async Task<bool> IsExist(string email)
		{
			try
			{
				var emply = await _context.Employees.Where(x => x.Email == email).FirstOrDefaultAsync();
				switch (emply)
				{
					case null:
						return false;
					default: 
						return true;

				}
				 
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return false;
			}
		}


		public async Task<Tuple<List<Employee>, int>> GetEmployees(int pageSize, int pageNumber)
		{
			try
			{
				var validFilter = new PaginationFilter(pageNumber, pageSize);
				var pagedData = await _context.Employees
								.Where(x => x.IsActive && !x.IsDeleted)
								.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
								.Take(validFilter.PageSize)
								.OrderByDescending(x => x.DateCreated)
								.ToListAsync();

				var rec = await _context.Employees.Where(x => x.IsActive && !x.IsDeleted).OrderByDescending(x => x.DateCreated).ToListAsync();
				var totalRecords = await _context.Employees.Where(x => x.IsActive && !x.IsDeleted).CountAsync();
				return new Tuple<List<Employee>, int>(rec, totalRecords); 
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return new Tuple<List<Employee>, int>(new List<Employee>(), 0);
			}
		}




		public async Task<Employee> GetEmployeeById(long employeeId)
		{
			try
			{
				var emply = await _context.Employees.Where(x=> x.Id == employeeId && x.IsActive && !x.IsDeleted).FirstOrDefaultAsync();
				return emply;
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return new Employee();
			}
		}


		public async Task<int> UpdateEmployeeInfo(long employeeId, Employee employee)
		{
			try
			{
				var emply = await _context.Employees.Where(x => x.Id == employeeId).FirstOrDefaultAsync();
				switch(emply == null)
				{
					case true: 
						return (int)DbInfo.NoIdFound;
				}
				emply.Salary = employee.Salary;
				emply.Name = employee.Name;
				emply.Email = employee.Email;
				emply.Phone = employee.Phone;
				emply.IsActive = true;
				emply.Department = employee.Department;
				emply.DateUpdated = DateTime.Now; 

				//var save = await _context.SaveChangesAsync(employee.IpAddress);
				var save = await _context.SaveChangesAsync();
				return save;
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return (int)DbInfo.ErrorThrown;
			}
		}




		public async Task<int> DeleteEmployeeInfo(long employeeId, string ipAddress)
		{
			try
			{
				var emply = await _context.Employees.Where(x => x.Id == employeeId).FirstOrDefaultAsync();
				switch (emply == null)
				{
					case true:
						return (int)DbInfo.NoIdFound;
				}
				emply.IsActive = false;
				emply.IsDeleted = true;
				emply.DateDeleted = DateTime.Now; 

				//var save = await _context.SaveChangesAsync(ipAddress);
				var save = await _context.SaveChangesAsync();
				return save;
			}
			catch (Exception ex)
			{
				LoggerMiddleware.LogError(ex.Message);
				return (int)DbInfo.ErrorThrown;
			}
		}


	}
}
