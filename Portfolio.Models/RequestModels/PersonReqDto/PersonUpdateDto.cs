using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Portfolio.Models.RequestModels.PersonReqDto
{
    public class PersonUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? Designation { get; set; }
        public string? Address { get; set; }
        public string? Country { get; set; }
        public string? Nationality { get; set; }
        public string? PostCode { get; set; }
        public string? IsAvailableInFreelance { get; set; }
        public int NumberOfYearsOfExperience { get; set; }
        public int NumberOfProjects { get; set; }
        public int NumberOfTechnologies { get; set; }
        public int NumberOfCodeCommits { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? LogoUrl { get; set; }
        public string? LinkedInLink { get; set; }
        public string? GitHubLink { get; set; }
        public string? YoutubeLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? VideoCVLink { get; set; }
    }
}