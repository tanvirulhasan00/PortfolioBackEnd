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
        public async Task<ApiResponse> Login([FromBody] LoginRequestDto request)
        {
            var response = new ApiResponse();

            var loginRes = await _unitOfWork.LocalUser.Login(request);
            if (loginRes.Success == false)
            {
                response.Success = loginRes.Success;
                response.StatusCode = loginRes.StatusCode;
                response.Message = loginRes.Message;
                return response;
            }

            response.Success = loginRes.Success;
            response.StatusCode = loginRes.StatusCode;
            response.Message = loginRes.Message;
            response.Result = loginRes.Result;

            return response;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<ApiResponse> Registration([FromBody] RegistrationRequestDto request)
        {
            var response = new ApiResponse();
            var ifUniqueUser = _unitOfWork.LocalUser.IsUniqueUser(request.UserName);
            if (!ifUniqueUser)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.Conflict;
                response.Message = "User already exits";
                return response;
            }
            var user = await _unitOfWork.LocalUser.Registration(request);
            if (user.Success == false)
            {
                response.Success = user.Success;
                response.StatusCode = user.StatusCode;
                response.Message = user.Message;
                return response;
            }
            await _unitOfWork.Save();

            response.Success = user.Success;
            response.StatusCode = user.StatusCode;
            response.Message = user.Message;
            //user.Result = "";
            response.Result = user.Result;

            return response;
        }
    }
}