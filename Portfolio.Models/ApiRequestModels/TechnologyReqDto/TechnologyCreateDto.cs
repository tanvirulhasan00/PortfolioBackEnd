using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Models.ApiRequestModels.TechnologyReqDto
{
    public class TechnologyCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? LogoUrl { get; set; }
        public int Active { get; set; }
    }
}