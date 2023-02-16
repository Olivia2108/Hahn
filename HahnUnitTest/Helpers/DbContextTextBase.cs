using Bogus.DataSets;
using HahnData.DataContext;
using HahnData.Repositories.Contracts;
using Microsoft.EntityFrameworkCore; 
using HahnData.Models;
using HahnData.Repositories; 
using HahnUnitTest.Fixtures; 
using MockQueryable.Moq;
using Moq;
using static Bogus.DataSets.Name;

namespace HahnUnitTest.Helpers
{
	 
	public class DbContextTextBase : IDisposable
	{
		protected readonly HahnContext _context;
		public DbContextTextBase()
		{
			var options = new DbContextOptionsBuilder<HahnContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
			_context = new HahnContext(options);

			_context.Database.EnsureCreated();
			Seed(_context);
		}
		 
		public void Dispose()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
		private void Seed(HahnContext context)
		{
			var staff = EmployeeFixture.GenerateData(10);

			foreach (var employee in staff)
			{
				context.Employees.AddAsync(employee);
			}

			 
			 
			context.SaveChangesAsync();
		}

		public static Tuple<IEmployeeRepository, List<Employee>, Mock<HahnContext>> GetMock()
		{
			var staff = Fixtures.EmployeeFixture.GenerateData(10);


			var data = staff.AsQueryable().BuildMock();
			var mockDbContext = new Mock<HahnContext>();

			var mockSet = new Mock<DbSet<Employee>>();

			mockSet
				.As<IQueryable<Employee>>()
				.Setup(m => m.Provider)
				.Returns(data.Provider);

			mockSet
				.As<IQueryable<Employee>>()
				.Setup(m => m.ElementType)
				.Returns(data.ElementType);

			mockSet
				.As<IQueryable<Employee>>()
				.Setup(m => m.Expression)
				.Returns(data.Expression);

			mockSet
				.As<IQueryable<Employee>>()
				.Setup(m => m.GetEnumerator())
				.Returns(data.GetEnumerator());
			 

			mockDbContext
				.Setup(o => o.Employees)
				.Returns(() => mockSet.Object);


			mockDbContext
				.Setup(x => x.SaveChangesAsync(""))
				.Returns(Task.FromResult(1));


			IEmployeeRepository repository = new EmployeeRepository(mockDbContext.Object);

			return new Tuple<IEmployeeRepository, List<Employee>, Mock<HahnContext>>(repository, staff, mockDbContext);
		}

	}
}
