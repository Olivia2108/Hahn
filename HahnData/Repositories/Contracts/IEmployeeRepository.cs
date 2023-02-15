using HahnData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnData.Repositories.Contracts
{
	public interface IEmployeeRepository
	{
		Task<long> AddEmployee(Employee data);
		Task<bool> IsExist(string email);
		Task<List<Employee>> GetEmployees();
		Task<Employee> GetEmployeeById(long employeeId);
		Task<int> UpdateEmployeeInfo(long employeeId, Employee employee);
		Task<int> DeleteEmployeeInfo(long employeeId, string ipAddress);
	}
}
