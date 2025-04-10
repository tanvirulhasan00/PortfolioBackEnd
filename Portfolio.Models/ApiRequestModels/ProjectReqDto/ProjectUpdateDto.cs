using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Portfolio.Models.ApiRequestModels.ProjectReqDto
{
    public class ProjectUpdateDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Catagory { get; set; }
        public IFormFile? ImageUrl { get; set; }
        public string? LiveLink { get; set; }
        public string? GithubLink { get; set; }
        public int Active { get; set; }
    }
}