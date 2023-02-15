﻿using System;
using System.Collections.Generic;

namespace HahnData.Models
{
    public partial class AuditTrail
    {
        public int Id { get; set; }
        public string? IpAddress { get; set; }
        public string? Type { get; set; }
        public string? TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? AffectedColumns { get; set; }
        public string? PrimaryKey { get; set; }
    }
}
