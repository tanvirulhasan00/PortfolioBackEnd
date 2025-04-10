using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.ApiRequestModels.EducationReqDto;
using Portfolio.Models.ApiRequestModels.GenericRequestModels;
using Portfolio.Models.ApiResponseModels.ApiResponse;
using Portfolio.RepositoryConfig.IRepositories;
using Portfolio.Models.ApiRequestModels.ProjectReqDto;
using Portfolio.Models.ApiRequestModels.TechnologyReqDto;
using Portfolio.Models.ApiRequestModels.CustomerMessageReqDto;
using Portfolio.Models.ApiRequestModels.ProjectAndTechnologyReqDto;

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/project-and-technology")]
    [ApiVersion("1.0")]
    public class ProjectAndTechnologyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse response;

        public ProjectAndTechnologyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ApiResponse> GetAllProjectAndTechnology(CancellationToken cancellationToken)
        {
            var req = new GenericRequest<ProjectAndTechnology>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            var projectAndTechnology = await _unitOfWork.ProjectAndTechnology.GetAllAsync(req);
            if (projectAndTechnology == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Data not found.";
                return response;
            }
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Successful";
            response.Result = projectAndTechnology;
            return response;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ApiResponse> GetProjectAndTechnology(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Valid id required";
                return response;
            }
            var req = new GenericRequest<ProjectAndTechnology>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var projectAndTechnology = await _unitOfWork.ProjectAndTechnology.GetAsync(req);
                if (projectAndTechnology == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Result = projectAndTechnology;
                return response;
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
        [Route("create")]
        public async Task<ApiResponse> CreateProjectAndTechnology(ProjectAndTechnologyCreateDto request)
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
                ProjectAndTechnology projectAndTechnology = new()
                {
                    ProjectId = request.ProjectId,
                    TechnologyId = request.TechnologyId,
                };

                await _unitOfWork.ProjectAndTechnology.AddAsync(projectAndTechnology);
                int result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Project-And-Technology created successfully";
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Failed to create.";
                }
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
        [Route("delete")]
        public async Task<ApiResponse> DeleteProjectAndTechnology(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Valid id required";
                return response;
            }
            var req = new GenericRequest<ProjectAndTechnology>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var projectAndTechnology = await _unitOfWork.ProjectAndTechnology.GetAsync(req);
                _unitOfWork.ProjectAndTechnology.Remove(projectAndTechnology);
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Project-and-technology deleted successfully";
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