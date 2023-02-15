﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HahnData.Middleware.Constants
{
	public class ResponseConstants
	{
		public const string NotFound = "No record was found";
		public const string Found = "Records returned";
		public const string Saved = "Record saved successfully";
		public const string IsExist = "Employee with this email already exist";
		public const string NotSaved = "Record was not saved, pls try again later";
		public const string Updated = "Record was updated successfully";
		public const string NotUpdated = "Record was not updated, pls try again later";
		public const string Deleted = "Record was deleted successfully";
		public const string NotDeleted = "Record was not deleted, pls provide a valid ID";
	}
}
