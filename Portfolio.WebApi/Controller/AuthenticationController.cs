using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.LoginReqDto;
using Portfolio.Models.RequestModels.RegistrationReqDto;
using Portfolio.Models.ResponseModels.ApiResponseModels;
using Portfolio.Models.ResponseModels.LoginResDto;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.WebApi.Controller
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
        public async Task<ApiResponse<LoginResponseDto>> Login([FromBody] LoginRequestDto request)
        {
            var response = new ApiResponse<LoginResponseDto>();

            var loginRes = await _unitOfWork.LocalUser.Login(request);
            if (loginRes.Success == false)
            {
                response.Success = loginRes.Success;
                response.StatusCode = loginRes.StatusCode;
                response.Message = loginRes.Message;
                response.Error = loginRes.Error;
                return response;
            }

            response.Success = loginRes.Success;
            response.StatusCode = loginRes.StatusCode;
            response.Message = loginRes.Message;
            response.SingleData = loginRes.SingleData;

            return response;
        }

        [HttpPost]
        [Route("registration")]
        public async Task<ApiResponse<LocalUser>> Registration([FromBody] RegistrationRequestDto request)
        {
            var response = new ApiResponse<LocalUser>();
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
                response.Error = user.Error;
                return response;
            }
            await _unitOfWork.Save();

            response.Success = user.Success;
            response.StatusCode = user.StatusCode;
            response.Message = user.Message;
            user.SingleData.Password = "";
            response.SingleData = user.SingleData;

            return response;
        }
    }
}