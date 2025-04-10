using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.ApiRequestModels.ExperienceReqDto
{
    public class ExperienceCreateDto
    {
        public string? Designation { get; set; }
        public string? CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int Active { get; set; }
    }
}