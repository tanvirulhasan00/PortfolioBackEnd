using System;
using Portfolio.Models.DbModels;

namespace Portfolio.Models.ResponseModels.LoginResDto
{
  public class LoginResponseDto
  {
    public LocalUser? User { get; set; }
    public string? Token { get; set; }
  }
}