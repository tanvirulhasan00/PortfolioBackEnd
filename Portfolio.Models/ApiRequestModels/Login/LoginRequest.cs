using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models.PortfolioModels.AuthenticationModels;

namespace Portfolio.Models.ApiRequestModels.Login
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

    }
}