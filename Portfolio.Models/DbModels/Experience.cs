using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.DbModels
{
    public class Experience
    {
        [Key]
        public int Id { get; set; }
        public string? Designation { get; set; }
        public string? CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public bool IsActive { get; set; }

    }
}