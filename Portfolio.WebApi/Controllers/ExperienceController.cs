using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.ApiRequestModels.ExperienceReqDto;
using Portfolio.Models.ApiRequestModels.GenericRequestModels;
using Portfolio.Models.ApiResponseModels.ApiResponse;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/experience")]
    [ApiVersion("1.0")]
    public class ExperienceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse response;
        public ExperienceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ApiResponse> GetAllExperience(CancellationToken cancellationToken)
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
                var experience = await _unitOfWork.Experience.GetAllAsync(req);
                if (experience == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Result = experience;
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ApiResponse> GetExperience(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Id required.";
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
                var experience = await _unitOfWork.Experience.GetAsync(req);
                if (experience == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Result = experience;
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
            }

            return response;
        }


        [HttpPost]
        [Route("create")]
        public async Task<ApiResponse> CreateExperience([FromBody] ExperienceCreateDto request)
        {
            if (request == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Unsuccessful";
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
                    response.Message = "Experience created successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Failed to create";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<ApiResponse> UpdateExperience([FromBody] ExperienceUpdateDto request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Id required.";
                return response;
            }
            var req = new GenericRequest<Experience>
            {
                Expression = x => x.Id == request.Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var experienceInfo = await _unitOfWork.Experience.GetAsync(req);
                if (experienceInfo != null)
                {
                    experienceInfo.Designation = (request.Designation == "" || request.Designation == null) ? experienceInfo.Designation : request.Designation;
                    experienceInfo.CompanyName = (request.CompanyName == "" || request.CompanyName == null) ? experienceInfo.CompanyName : request.CompanyName;
                    experienceInfo.StartDate = request.StartDate;
                    experienceInfo.EndDate = request.EndDate;
                    experienceInfo.Active = request.Active;

                    _unitOfWork.Experience.Update(experienceInfo);
                }
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Experience updated successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Failed to update";
                    return response;
                }
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }

        }

        [HttpPost]
        [Route("delete")]
        public async Task<ApiResponse> DeleteExperience(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Id required";
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
                var experienceInfo = await _unitOfWork.Experience.GetAsync(req);
                _unitOfWork.Experience.Remove(experienceInfo);
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Experience deleted successfully";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Failed to delete";
                    return response;
                }
            }
            catch (TaskCanceledException ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.RequestTimeout;
                response.Message = ex.Message;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
                return response;
            }
        }
    }
}