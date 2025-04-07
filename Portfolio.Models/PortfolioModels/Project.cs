using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.PortfolioModels
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Catagory { get; set; }
        public string? ImageUrl { get; set; }
        public string? LiveLink { get; set; }
        public string? GithubLink { get; set; }
        public int IsActive { get; set; }
    }
}