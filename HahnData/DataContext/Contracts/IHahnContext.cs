using HahnData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnData.DataContext.Contracts
{
	public interface IHahnContext
	{
		DbSet<Employee> Employees { get; set; }
		DbSet<AuditTrail> AuditTrails { get; set; }
		Task<int> SaveChangesAsync(string? ipAddress);
	}
}
