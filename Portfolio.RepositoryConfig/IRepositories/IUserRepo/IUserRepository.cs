using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.LoginReqDto;
using Portfolio.Models.RequestModels.RegistrationReqDto;
using Portfolio.Models.ResponseModels.ApiResponseModels;
using Portfolio.Models.ResponseModels.LoginResDto;

namespace Portfolio.RepositoryConfig.IRepositories.IUserRepo
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string userName);
        Task<ApiResponse<LoginResponseDto>> Login(LoginRequestDto loginRequestDto);
        Task<ApiResponse<LocalUser>> Registration(RegistrationRequestDto registrationRequestDto);
    }
}