using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.ApiRequestModels.ProjectAndTechnologyReqDto
{
    public class ProjectAndTechnologyUpdateDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int TechnologyId { get; set; }

    }
}