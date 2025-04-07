using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.ApiRequestModels.ServiceReqDto
{
    public class ServiceCreateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int IsActive { get; set; }
    }
}