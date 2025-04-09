using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portfolio.DatabaseConfig.Data;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.ApiRequestModels.LoginReqDto;
using Portfolio.Models.ApiRequestModels.RegistrationReqDto;
using Portfolio.Models.ApiResponseModels.ApiResponse;

using Portfolio.RepositoryConfig.IRepositories.IUserRepo;
using Portfolio.Models.PortfolioModels.AuthenticationModels;
using Portfolio.Models.ApiResponseModels.LoginResDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Portfolio.RepositoryConfig.Repositories.UserRepo
{
  public class UserRepository : IUserRepository
  {
    private readonly PortfolioDbContext _context;
    private readonly string secretKey;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IWebHostEnvironment _env;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserRepository(PortfolioDbContext context, string configuration, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
    {
      _context = context;
      secretKey = configuration;
      _userManager = userManager;
      _roleManager = roleManager;
      _env = env;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ApiResponse> Login(LoginRequestDto request)
    {
      var response = new ApiResponse();
      var loginRes = new LoginResponseDto();
      try
      {
        var user = await _context.ApplicationUsers.FirstOrDefaultAsync(x => x.UserName.ToLower() == request.UserName.ToLower());
        bool isValid = await _userManager.CheckPasswordAsync(user, request.Password);
        if (user == null || isValid == false)
        {
          response.Success = false;
          response.StatusCode = HttpStatusCode.BadRequest;
          response.Message = "Username and password is incorrect!";
          return response;
        }

        //if user found generate jwt token 
        var roles = await _userManager.GetRolesAsync(user);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity([
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName.ToString()),
            new Claim(ClaimTypes.Role, roles.FirstOrDefault())
          ]),
          Expires = DateTime.UtcNow.AddDays(7),
          SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        loginRes.Token = tokenHandler.WriteToken(token);

        response.Success = true;
        response.StatusCode = HttpStatusCode.OK;
        response.Message = "Login Successful";
        response.Result = loginRes;
        return response;
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.StatusCode = HttpStatusCode.InternalServerError;
        response.Message = ex.Message;
        return response;
      }
    }

    public async Task<ApiResponse> Registration(RegistrationRequestDto request)
    {
      var response = new ApiResponse();
      ApplicationUser user = new()
      {
        UserName = request.FirstName + request.LastName,
        PhoneNumber = request.PhoneNumber,
        Email = request.Email,
      };

      //get root path of wwwroot
      var rootPath = _env.WebRootPath;

      //generate unique names for the files
      var profilePicName = Guid.NewGuid().ToString() + Path.GetExtension(request.ProfileImageUrl?.FileName);
      var logoName = Guid.NewGuid().ToString() + Path.GetExtension(request.LogoUrl?.FileName);
      var cvName = Guid.NewGuid().ToString() + Path.GetExtension(request.CVDownloadLink?.FileName);
      var videoCvName = Guid.NewGuid().ToString() + Path.GetExtension(request.VideoCVLink?.FileName);

      //combine root path with file names to create file paths
      var profilePicPath = Path.Combine(rootPath, "images", profilePicName);
      var logoPath = Path.Combine(rootPath, "images", logoName);
      var cvPath = Path.Combine(rootPath, "pdfs", cvName);
      var videoPath = Path.Combine(rootPath, "videos", videoCvName);

      // ensure the folders existance in wwwroot
      var folders = new[] { "images", "pdfs", "videos" };

      foreach (var folder in folders)
      {
        var fullPath = Path.Combine(rootPath, folder);
        Directory.CreateDirectory(fullPath); // Safe even if it already exists
      }

      //save pictures
      if (request.ProfileImageUrl != null)
      {
        await SaveFileAsync(request.ProfileImageUrl, profilePicPath);
      }
      if (request.LogoUrl != null)
      {
        await SaveFileAsync(request.LogoUrl, logoPath);
      }
      if (request.CVDownloadLink != null)
      {
        await SaveFileAsync(request.CVDownloadLink, cvPath);
      }
      if (request.VideoCVLink != null)
      {
        await SaveFileAsync(request.VideoCVLink, videoPath);
      }

      //create url for db
      var profilePicUrl = GetUrl(profilePicName, "images");
      var logoUrl = GetUrl(logoName, "images");
      var cvUrl = GetUrl(cvName, "pdfs");
      var videoCvUrl = GetUrl(videoCvName, "videos");


      try
      {
        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
          Person personData = new()
          {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Bio = request.Bio,
            Designation = request.Designation,
            Address = request.Address,
            Country = request.Country,
            Nationality = request.Nationality,
            PostCode = request.PostCode,
            AvailableInFreelance = request.AvailableInFreelance,
            NumberOfYearsOfExperience = request.NumberOfYearsOfExperience,
            NumberOfProjects = request.NumberOfProjects,
            NumberOfTechnologies = request.NumberOfTechnologies,
            NumberOfCodeCommits = request.NumberOfCodeCommits,
            ProfileImageUrl = profilePicUrl,
            LogoUrl = logoUrl,
            LinkedInLink = request.LinkedInLink,
            GitHubLink = request.GitHubLink,
            YoutubeLink = request.YoutubeLink,
            TwitterLink = request.TwitterLink,
            CVDownloadLink = cvUrl,
            VideoCVLink = videoCvUrl,
          };
          var person = await _context.Persons.AddAsync(personData);
          // await _context.SaveChangesAsync();
          if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
          {
            await _roleManager.CreateAsync(new IdentityRole("admin"));
          }
          await _userManager.AddToRoleAsync(user, "admin");

          response.Success = true;
          response.StatusCode = HttpStatusCode.Created;
          response.Message = "User created successfully.";
        }
        else
        {
          response.Success = false;
          response.StatusCode = HttpStatusCode.InternalServerError;
          response.Message = $"{string.Join("\n", result.Errors.Select(s => s.Code))}\n{string.Join("\n", result.Errors.Select(s => s.Description))}";
          response.Error = result.Errors;
        }
        return response;
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.StatusCode = HttpStatusCode.OK;
        response.Message = ex.Message;
        return response;
      }
    }

    static async Task SaveFileAsync(IFormFile file, string path)
    {
      using var stream = new FileStream(path, FileMode.Create);
      await file.CopyToAsync(stream);
    }
    string GetUrl(string fileName, string folderName)
    {
      var request = _httpContextAccessor.HttpContext?.Request;
      return $"{request?.Scheme}://{request?.Host}/{folderName}/{fileName}";
    }
  }
}