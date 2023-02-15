using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnData.Dto
{
	public class ApiResponseDto
	{
		public bool Success { get; set; }
		public Object? Data { get; set; }
		public string? Message { get; set; }
		public int TotalCount { get; set; }
	}

	 
}
