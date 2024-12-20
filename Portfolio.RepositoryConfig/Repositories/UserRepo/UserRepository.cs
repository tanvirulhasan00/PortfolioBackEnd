using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.LoginReqDto;
using Portfolio.Models.RequestModels.RegistrationReqDto;
using Portfolio.Models.ResponseModels.ApiResponseModels;
using Portfolio.Models.ResponseModels.LoginResDto;
using Portfolio.RepositoryConfig.IRepositories.IUserRepo;

namespace Portfolio.RepositoryConfig.Repositories.UserRepo
{
  public class UserRepository : IUserRepository
  {
    private readonly PortfolioDbContext _context;
    private readonly string secretKey;
    public UserRepository(PortfolioDbContext context, string configuration)
    {
      _context = context;
      secretKey = configuration;
    }
    public bool IsUniqueUser(string userName)
    {
      var user = _context.LocalUsers.FirstOrDefault(x => x.UserName == userName);
      if (user == null)
      {
        return true;
      }
      return false;
    }

    public async Task<ApiResponse<LoginResponseDto>> Login(LoginRequestDto loginRequestDto)
    {
      var response = new ApiResponse<LoginResponseDto>();
      var loginRes = new LoginResponseDto();
      try
      {
        var user = await _context.LocalUsers.FirstOrDefaultAsync(x => x.UserName != null && x.UserName.ToLower() == loginRequestDto.UserName.ToLower() && x.Password == loginRequestDto.Password);
        if (user == null)
        {
          response.Success = false;
          response.StatusCode = HttpStatusCode.BadRequest;
          response.Message = "Username and password is incorrect!";
          return response;
        }

        //if user found generate jwt token 
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity([
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
          ]),
          Expires = DateTime.UtcNow.AddDays(7),
          SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        loginRes.Token = tokenHandler.WriteToken(token);
        user.Password = "";
        loginRes.User = user;

        response.Success = true;
        response.StatusCode = HttpStatusCode.OK;
        response.Message = "Successful";
        response.SingleData = loginRes;
        return response;
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.StatusCode = HttpStatusCode.InternalServerError;
        response.Message = ex.Message;
        response.Error = ex;
        return response;
      }
    }

    public async Task<ApiResponse<LocalUser>> Registration(RegistrationRequestDto registrationRequestDto)
    {
      var response = new ApiResponse<LocalUser>();
      var user = new LocalUser
      {
        UserName = registrationRequestDto.UserName,
        Password = registrationRequestDto.Password,
        Name = registrationRequestDto.Name,
        Role = registrationRequestDto.Role,
      };
      try
      {
        await _context.LocalUsers.AddAsync(user);

        response.Success = true;
        response.StatusCode = HttpStatusCode.OK;
        response.Message = "Successful";
        response.SingleData = user;
        return response;
      }
      catch (Exception ex)
      {
        response.Success = false;
        response.StatusCode = HttpStatusCode.OK;
        response.Message = ex.Message;
        response.Error = ex;
        return response;
      }
    }
  }
}