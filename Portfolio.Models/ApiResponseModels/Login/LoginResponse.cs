using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models.PortfolioModels.AuthenticationModels;

namespace Portfolio.Models.ApiResponseModels.Login
{
    public class LoginResponse
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}