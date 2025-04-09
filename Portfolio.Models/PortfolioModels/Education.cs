using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.PortfolioModels
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        public string? Degree { get; set; }
        public string? Institute { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int Active { get; set; }

    }
}