using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.ApiRequestModels.LoginReqDto;
using Portfolio.Models.ApiRequestModels.RegistrationReqDto;
using Portfolio.Models.ApiResponseModels.ApiResponse;
using Portfolio.Models.ApiResponseModels.LoginResDto;
using Portfolio.RepositoryConfig.IRepositories;
using System.Text.RegularExpressions;

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/authentication")]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthenticationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        [Route("login")]
        public async Task<ApiResponse> Login(LoginRequestDto request)
        {
            var response = new ApiResponse();

            var loginRes = await _unitOfWork.Auth.Login(request);
            response = loginRes;
            return response;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<ApiResponse> Registration(RegistrationRequestDto request)
        {
            var response = new ApiResponse();

            var PasswordRegex = @"^(?=(.*[A-Z]))(?=(.*\d))(?=(.*\W))(?=.{6,})[A-Za-z\d\W]*$";
            var regex = new Regex(PasswordRegex);
            var validPassword = regex.IsMatch(request.Password);
            if (!validPassword)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Password must be at least 6 characters long, include at least one uppercase letter, one digit, and one non-alphanumeric character.";
                return response;
            }

            response = await _unitOfWork.Auth.Registration(request);
            return response;
        }
    }
}