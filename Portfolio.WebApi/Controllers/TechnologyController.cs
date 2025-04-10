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

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/technology")]
    [ApiVersion("1.0")]
    public class TechnologyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse response;

        public TechnologyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ApiResponse> GetAllTechnology(CancellationToken cancellationToken)
        {
            var req = new GenericRequest<Technology>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            var projects = await _unitOfWork.Technology.GetAllAsync(req);
            if (projects == null)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Data not found.";
                return response;
            }
            response.Success = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = "Successful";
            response.Result = projects;
            return response;
        }

        [HttpGet]
        [Route("get")]
        public async Task<ApiResponse> GetTechnology(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Valid id required";
                return response;
            }
            var req = new GenericRequest<Technology>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var projects = await _unitOfWork.Technology.GetAsync(req);
                if (projects == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Result = projects;
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
        public async Task<ApiResponse> CreateTechnology(TechnologyCreateDto request)
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
                var logoUrl = "";
                if (request.LogoUrl != null)
                {
                    logoUrl = await _unitOfWork.File.FileUpload(request.LogoUrl, "images/technologys");
                }

                Technology technology = new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    LogoUrl = logoUrl,
                    Active = request.Active,
                };

                await _unitOfWork.Technology.AddAsync(technology);
                int result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Technology created successfully";
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
        [Route("update")]
        public async Task<ApiResponse> UpdateTechnology(TechnologyUpdateDto request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Valid id required";
                return response;
            }
            var req = new GenericRequest<Technology>
            {
                Expression = x => x.Id == request.Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var technologyInfo = await _unitOfWork.Technology.GetAsync(req);
                if (technologyInfo != null)
                {
                    if (!string.IsNullOrEmpty(technologyInfo.LogoUrl))
                    {
                        _unitOfWork.File.DeleteFile(technologyInfo.LogoUrl);
                    }
                    var logoUrl = "";
                    if (request.LogoUrl != null)
                    {
                        logoUrl = await _unitOfWork.File.FileUpload(request.LogoUrl, "images/technologys");
                    }

                    technologyInfo.Name = (request.Name == "" || request.Name == null) ? technologyInfo.Name : request.Name;
                    technologyInfo.Description = (request.Description == "" || request.Description == null) ? technologyInfo.Description : request.Description;
                    technologyInfo.LogoUrl = request.LogoUrl == null ? technologyInfo.LogoUrl : logoUrl;
                    technologyInfo.Active = request.Active;

                    _unitOfWork.Technology.Update(technologyInfo);
                }
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Technology updated successfully";
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
        public async Task<ApiResponse> DeleteTechnology(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Valid id required";
                return response;
            }
            var req = new GenericRequest<Technology>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var projectInfo = await _unitOfWork.Technology.GetAsync(req);
                _unitOfWork.Technology.Remove(projectInfo);
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Technology deleted successfully";
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