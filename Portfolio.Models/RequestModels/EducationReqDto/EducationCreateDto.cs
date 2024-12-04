using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.RequestModels.EducationReqDto
{
    public class EducationCreateDto
    {
        public string? Degree { get; set; }
        public string? Institute { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int IsActive { get; set; }
    }
}