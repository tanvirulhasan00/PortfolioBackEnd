using Microsoft.AspNetCore.Http;

namespace Portfolio.Models.ApiRequestModels.RegistrationReqDto
{
  public class RegistrationRequestDto
  {
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Bio { get; set; }
    public string? Designation { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Nationality { get; set; }
    public string? PostCode { get; set; }
    public int AvailableInFreelance { get; set; }
    public int NumberOfYearsOfExperience { get; set; }
    public int NumberOfProjects { get; set; }
    public int NumberOfTechnologies { get; set; }
    public int NumberOfCodeCommits { get; set; }
    public IFormFile? ProfileImageUrl { get; set; }
    public IFormFile? LogoUrl { get; set; }
    public string? LinkedInLink { get; set; }
    public string? GitHubLink { get; set; }
    public string? YoutubeLink { get; set; }
    public string? TwitterLink { get; set; }
    public IFormFile? CVDownloadLink { get; set; }
    public IFormFile? VideoCVLink { get; set; }
  }
}