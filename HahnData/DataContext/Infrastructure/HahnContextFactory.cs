using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnData.DataContext.Infrastructure
{
	public class HahnContextFactory : HahnDesignTimeDbContextFactoryBase<HahnContext>
	{
		protected override HahnContext CreateNewInstance(DbContextOptions<HahnContext> options)
		{
			return new HahnContext(options);
		}
	}
}
