using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.BestPractices.DTOs
{
    public class DataFilterDto
    {  
        public string[]? Tags { get; set; }
        public DateTime? StartDate { get; set; }
        public string? SortBy { get; set; }
    }
}