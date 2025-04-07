using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.PortfolioModels
{
    public class ProjectAndTechnology
    {
        [Key]
        public int Id { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }
        public int TechnologyId { get; set; }
        [ForeignKey("TechnologyId")]
        public Technology Technology { get; set; }
    }
}