using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.ApiRequestModels.TechnologyReqDto
{
    public class TechnologyCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public int IsActive { get; set; }
    }
}