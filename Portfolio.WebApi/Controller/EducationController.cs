using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.EducationReqDto;
using Portfolio.Models.RequestModels.GenericRequestModels;
using Portfolio.Models.ResponseModels.ApiResponseModels;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.WebApi.Controller
{
    [ApiController]
    [Route("api/education")]
    public class EducationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse<Education> response;

        public EducationController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            response = new ApiResponse<Education>();
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ApiResponse<Education>> GetAllEducation(CancellationToken cancellationToken)
        {
            var req = new GenericRequest<Education>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            var education = await _unitOfWork.Education.GetAll(req);
            if (education == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.NoContent;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Successful";
            response.Data = education;
            return response;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ApiResponse<Education>> GetEducation(int Id, CancellationToken cancellationToken)
        {
            if (Id == 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }
            var req = new GenericRequest<Education>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var education = await _unitOfWork.Education.Get(req);
                if (education == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.Error = new { message = "Unsuccessful" };
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.SingleData = education;
                return response;
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
        [Route("Create")]
        public async Task<ApiResponse<Education>> CreateEducation([FromBody] EducationCreateDto request)
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
                var model = _mapper.Map<Education>(request);
                await _unitOfWork.Education.AddAsync(model);
                int result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Successful";
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Error = new { message = "Unsuccessful" };
                }
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
        [Route("Update")]
        public async Task<ApiResponse<Education>> UpdateEducation(int Id, [FromBody] EducationUpdateDto request, CancellationToken cancellationToken)
        {
            if (request == null || Id == 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }
            var req = new GenericRequest<Education>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var educationInfo = await _unitOfWork.Education.Get(req);
                if (educationInfo != null)
                {
                    educationInfo.Degree = (request.Degree == "" || request.Degree == null) ? educationInfo.Degree : request.Degree;
                    educationInfo.Institute = (request.Institute == "" || request.Institute == null) ? educationInfo.Institute : request.Institute;
                    educationInfo.StartDate = request.StartDate;
                    educationInfo.EndDate = request.EndDate;

                    _unitOfWork.Education.Update(educationInfo);
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
        public async Task<ApiResponse<Education>> RemoveEducation(int Id, CancellationToken cancellationToken)
        {
            if (Id == 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }
            var req = new GenericRequest<Education>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var educationInfo = await _unitOfWork.Education.Get(req);
                _unitOfWork.Education.Remove(educationInfo);
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
    }
}