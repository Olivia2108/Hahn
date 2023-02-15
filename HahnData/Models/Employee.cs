using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HahnData.Models
{
    public partial class Employee
	{
        [Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }
        public string? Name { get; set; }  
        public string? Email { get; set; } 
        public string? Phone { get; set; } 
        public string? Department { get; set; }  
        public string? IpAddress { get; set; }  
        public bool IsActive { get; set; }  
        public decimal Salary { get; set; }
        public DateTime DateCreated { get; set; } 
        public DateTime DateUpdated { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime DateDeleted { get; set; }
    }
}
