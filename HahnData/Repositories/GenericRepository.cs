using HahnData.DataContext;
using HahnData.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnData.Repositories
{
	public class GenericRepository<TModel> : IGenericRepository<TModel> where TModel : class
	{
		private readonly HahnContext _context;

		public GenericRepository(HahnContext context)
		{
			this._context = context;
		}


		public async Task<List<TModel>> GetEmployees()
		{
			try
			{

				//var result = await context.Vehicles.ToListAsync();
				var result = await _context.Set<TModel>().ToListAsync();
				return result;
			}
			catch (Exception ex)
			{

				throw;
			}
		}
	}
}
