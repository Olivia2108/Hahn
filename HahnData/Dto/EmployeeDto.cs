using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnData.Dto
{
	public class EmployeeDto
	{
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; } 
		public string? Department { get; set; } 
		public decimal Salary { get; set; }
		public string? IpAddress { get; set; }
	}


	public class EmployeeValidator : AbstractValidator<EmployeeDto>
	{
		public EmployeeValidator()
		{

			RuleFor(x => x.Salary).GreaterThan(0).NotNull().WithMessage("Salary must be greater then 0.");
			RuleFor(x => x.Phone).NotNull().Length(11);
			RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required.").EmailAddress().WithMessage("A valid email address is required."); 
			RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Employee Name is required.");
			RuleFor(x => x.Department).NotNull().NotEmpty().WithMessage("Department information is required."); 
			RuleFor(x => x.IpAddress).NotNull().NotEmpty().WithMessage("IpAddress is required."); 


		}

		protected override bool PreValidate(ValidationContext<EmployeeDto> context, ValidationResult result)
		{
			if (context.InstanceToValidate == null)
			{
				result.Errors.Add(new ValidationFailure("", "Please ensure a model was supplied."));
				return false;
			}
			return true;
		}
	}
}
