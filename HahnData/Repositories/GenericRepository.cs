using HahnData.DataContext;
using HahnData.Repositories.Contracts;
using HahnDomain.Middleware.Exceptions;
using Microsoft.EntityFrameworkCore;
using NLog;
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
				 
				var result = await _context.Set<TModel>().ToListAsync();
				return result;
			}
			catch (Exception ex)
			{

				LoggerMiddleware.LogError(ex.Message);
			}
			return new List<TModel>();
		}
	}
}
