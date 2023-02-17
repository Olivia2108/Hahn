using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Security.Cryptography.Xml; 
using HahnData.DataContext.Contracts;
using HahnData.Dto;
using HahnData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using static HahnData.Middleware.Enums.GeneralEnums;

namespace HahnData.DataContext
{
    public partial class HahnContext : DbContext, IHahnContext
	{ 

		public HahnContext()
        {
        }

        public HahnContext(DbContextOptions<HahnContext> options) : base(options)
        {

		}

        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                //optionsBuilder.UseSqlServer(config.GetValue<string>("ConnectionStrings:HahnConn"));
                optionsBuilder.UseSqlServer("Server=(local); DataBase=Hahn;Integrated Security=true");
            }
        }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Department).HasMaxLength(200);

                entity.Property(e => e.Email).HasMaxLength(200);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(200);

                entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)"); 

				entity.Property(e => e.IsActive).HasDefaultValue(true);

				entity.Property(e => e.DateCreated).HasDefaultValueSql("getdate()");
            });
			 

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);



		public virtual async Task<int> SaveChangesAsync(string? ipAddress)
		{
			PerformEntityAudit();
			OnBeforeSaveChanges(ipAddress);
			var result = await base.SaveChangesAsync();
			return result;
		}

		private void OnBeforeSaveChanges(string? ipAddress = null)
		{
			ChangeTracker.DetectChanges();
			var auditEntries = new List<AuditTrailDto>();
			foreach (var entry in ChangeTracker.Entries())
			{
				if (entry.Entity is AuditTrail || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
					continue;
				var auditEntry = new AuditTrailDto(entry)
				{
					TableName = entry.Entity.GetType().Name, 
					IpAddress = ipAddress
				};
				auditEntries.Add(auditEntry);
				foreach (var property in entry.Properties)
				{
					var propertyName = property.Metadata.Name;
					if (property.Metadata.IsPrimaryKey())
					{
						auditEntry.KeyValues[propertyName] = property.CurrentValue;
						continue;
					}
					switch (entry.State)
					{
						case EntityState.Added:
							auditEntry.AuditType = AuditType.Create;
							auditEntry.NewValues[propertyName] = property.CurrentValue;
							break;
						case EntityState.Deleted:
							auditEntry.AuditType = AuditType.Delete;
							auditEntry.OldValues[propertyName] = property.OriginalValue;
							break;
						case EntityState.Modified:
							if (property.IsModified)
							{
								auditEntry.ChangedColumns.Add(propertyName);
								auditEntry.AuditType = AuditType.Update;
								auditEntry.OldValues[propertyName] = property.OriginalValue;
								auditEntry.NewValues[propertyName] = property.CurrentValue;
							}
							break;
						case EntityState.Detached:
							break;
						case EntityState.Unchanged:
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
				}
			}
			foreach (var auditEntry in auditEntries)
			{
				AuditTrails?.Add(auditEntry.ToAudit());
			}
		}

		private void PerformEntityAudit()
		{
			foreach (var entry in ChangeTracker.Entries<Employee>())
			{
				switch (entry.State)
				{
					case EntityState.Added:
						var currentDateTime = DateTime.Now; 
						entry.Entity.DateCreated = currentDateTime;
						entry.Entity.IsDeleted = false;
						entry.Entity.IsActive = true;
						break;

					case EntityState.Modified:
						entry.Entity.IsActive = true;
						break;

					case EntityState.Deleted:
						entry.State = EntityState.Modified; 
						entry.Entity.DateDeleted = DateTime.Now;
						entry.Entity.IsDeleted = true;
						entry.Entity.IsActive = false;
						break;
				}
			} 
		}

	}
}
