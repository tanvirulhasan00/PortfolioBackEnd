using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.ApiRequestModels.LoginReqDto;
using Portfolio.Models.ApiRequestModels.RegistrationReqDto;
using Portfolio.Models.ApiResponseModels.ApiResponse;


namespace Portfolio.RepositoryConfig.IRepositories.IUserRepo
{
    public interface IUserRepository
    {
        Task<ApiResponse> Login(LoginRequestDto loginRequestDto);
        Task<ApiResponse> Registration(RegistrationRequestDto registrationRequestDto);
    }
}