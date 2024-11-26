using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.ExperienceReqDto;
using Portfolio.Models.RequestModels.GenericRequestModels;
using Portfolio.Models.RequestModels.PersonReqDto;
using Portfolio.Models.RequestModels.ServiceReqDto;
using Portfolio.Models.ResponseModels.ApiResponseModels;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.WebApi.Controller
{
    [ApiController]
    [Route("api/experience")]
    public class ExperienceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse<Experience> response;
        public ExperienceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            response = new ApiResponse<Experience>();
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ApiResponse<Experience>> GetAllExperience(CancellationToken cancellationToken)
        {

            var req = new GenericRequest<Experience>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var experience = await _unitOfWork.Experience.GetAll(req);
                if (experience == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.Error = new { message = "Unsuccessful" };
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Data = experience;
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Error = ex;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Error = ex;
            }

            return response;
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ApiResponse<Experience>> CreateExperience([FromBody] ExperienceCreateDto request)
        {
            if (request == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }
            try
            {
                var model = _mapper.Map<Experience>(request);
                await _unitOfWork.Experience.AddAsync(model);
                int result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Successful";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Error = new { message = "Unsuccessful" };
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Error = ex;
                return response;
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<ApiResponse<Experience>> UpdateExperience(int Id, [FromBody] ExperienceUpdateDto request, CancellationToken cancellationToken)
        {
            if (request == null || Id == 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }
            var req = new GenericRequest<Experience>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var experienceInfo = await _unitOfWork.Experience.Get(req);
                if (experienceInfo != null)
                {
                    experienceInfo.Designation = (request.Designation == "" || request.Designation == null) ? experienceInfo.Designation : request.Designation;
                    experienceInfo.CompanyName = (request.CompanyName == "" || request.CompanyName == null) ? experienceInfo.CompanyName : request.CompanyName;
                    experienceInfo.StartDate = request.StartDate;
                    experienceInfo.EndDate = request.EndDate;

                    _unitOfWork.Experience.Update(experienceInfo);
                }
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Successful";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Error = new { message = "Unsuccessful" };
                    return response;
                }
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Error = ex;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Error = ex;
                return response;
            }

        }

        [HttpPost]
        [Route("Remove")]
        public async Task<ApiResponse<Experience>> RemoveExperience(int Id, CancellationToken cancellationToken)
        {
            if (Id == 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }

            var req = new GenericRequest<Experience>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var experienceInfo = await _unitOfWork.Experience.Get(req);
                _unitOfWork.Experience.Remove(experienceInfo);
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Successful";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Unsuccessful";
                    return response;
                }
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Error = ex;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Error = ex;
                return response;
            }
        }
    }
}