using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Portfolio.DatabaseConnection.Data;
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.LoginReqDto;
using Portfolio.Models.RequestModels.RegistrationReqDto;
using Portfolio.Models.ResponseModels.LoginResDto;
using Portfolio.RepositoryConfig.IRepositories.IUserRepo;

namespace Portfolio.RepositoryConfig.Repositories.UserRepo
{
  public class UserRepository : IUserRepository
  {
    private readonly PortfolioDbContext _context;
    private readonly string secretKey;
    public UserRepository(PortfolioDbContext context, IConfiguration configuration)
    {
      _context = context;
      secretKey = configuration["TokenSetting:SecretKey"] ?? throw new InvalidOperationException("SecretKey is not configured.");
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

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
      var response = new LoginResponseDto();
      var user = await _context.LocalUsers.FirstOrDefaultAsync(x => x.UserName != null && x.UserName.ToLower() == loginRequestDto.UserName.ToLower() && x.Password == loginRequestDto.Password);
      if (user == null)
      {
        return response;
      }

      //if user found generate jwt token 
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(secretKey);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]{
          new Claim(ClaimTypes.Name, user.Id.ToString()),
          new Claim(ClaimTypes.Role, user.Role)
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      response.Token = tokenHandler.WriteToken(token);
      response.User = user;
      return response;
    }

    public async Task<LocalUser> Registration(RegistrationRequestDto registrationRequestDto)
    {
      var user = new LocalUser
      {
        UserName = registrationRequestDto.UserName,
        Password = registrationRequestDto.Password,
        Name = registrationRequestDto.Name,
        Role = registrationRequestDto.Role,
      };
      await _context.LocalUsers.AddAsync(user);
      user.Password = "";
      return user;
    }
  }
}