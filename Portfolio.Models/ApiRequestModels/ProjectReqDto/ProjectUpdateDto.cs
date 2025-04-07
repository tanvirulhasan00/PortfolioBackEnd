using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.ApiRequestModels.ProjectReqDto
{
    public class ProjectUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Catagory { get; set; }
        public string? ImageUrl { get; set; }
        public string? LiveLink { get; set; }
        public string? GithubLink { get; set; }
        public int IsActive { get; set; }
    }
}