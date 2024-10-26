using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Portfolio.Models.PortfolioModels
{
    public class Person
    {
        public int PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Designation { get; set; }
        public DateTime DateOfBirth { get; set; } = new DateTime(1990, 1, 1);
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? ZipCode { get; set; }
        public string? Bio { get; set; }
        public string? LinkedInLink { get; set; }
        public string? GitHubLink { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? VideoLink { get; set; }
    }
}