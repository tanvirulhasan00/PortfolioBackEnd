using System;
using Portfolio.Models.PortfolioModels.AuthenticationModels;

namespace Portfolio.Models.ApiResponseModels.LoginResDto
{
  public class LoginResponseDto
  {
    public LocalUser? User { get; set; }
    public string? Token { get; set; }
  }
}