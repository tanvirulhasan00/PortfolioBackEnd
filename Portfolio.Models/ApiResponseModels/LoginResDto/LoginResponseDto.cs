using System;
using Portfolio.Models.PortfolioModels.AuthenticationModels;

namespace Portfolio.Models.ApiResponseModels.LoginResDto
{
  public class LoginResponseDto
  {
    // public ApplicationUser? User { get; set; }
    public string? Token { get; set; }
  }
}