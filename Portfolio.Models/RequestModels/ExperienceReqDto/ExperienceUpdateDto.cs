using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.RequestModels.ExperienceReqDto
{
    public class ExperienceUpdateDto
    {
        public string? Designation { get; set; }
        public string? CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}