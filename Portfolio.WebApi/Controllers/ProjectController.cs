using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.ApiRequestModels.EducationReqDto;
using Portfolio.Models.ApiRequestModels.GenericRequestModels;
using Portfolio.Models.ApiResponseModels.ApiResponse;
using Portfolio.RepositoryConfig.IRepositories;
using Portfolio.Models.ApiRequestModels.ProjectReqDto;

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/project")]
    [ApiVersion("1.0")]
    public class ProjectController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse response;

        public ProjectController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ApiResponse> GetAllProject(CancellationToken cancellationToken)
        {
            var req = new GenericRequest<Project>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            var projects = await _unitOfWork.Project.GetAllAsync(req);
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
        public async Task<ApiResponse> GetProject(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Id required";
                return response;
            }
            var req = new GenericRequest<Project>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var projects = await _unitOfWork.Project.GetAsync(req);
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
        public async Task<ApiResponse> CreateProject(ProjectCreateDto request)
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
                // var model = _mapper.Map<Project>(request);
                var imageUrl = "";
                if (request.ImageUrl != null)
                {
                    imageUrl = await _unitOfWork.File.FileUpload(request.ImageUrl, "images/projects");
                }
                // model.ImageUrl = imageUrl;

                Project project = new()
                {
                    Title = request.Title,
                    Description = request.Description,
                    Catagory = request.Catagory,
                    LiveLink = request.LiveLink,
                    GithubLink = request.GithubLink,
                    Active = request.Active,
                    ImageUrl = imageUrl,
                };

                await _unitOfWork.Project.AddAsync(project);
                int result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Project created successfully";
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
        public async Task<ApiResponse> UpdateProject(ProjectUpdateDto request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Id required";
                return response;
            }
            var req = new GenericRequest<Project>
            {
                Expression = x => x.Id == request.Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var projectInfo = await _unitOfWork.Project.GetAsync(req);
                if (projectInfo != null)
                {
                    if (!string.IsNullOrEmpty(projectInfo.ImageUrl))
                    {
                        _unitOfWork.File.DeleteFile(projectInfo.ImageUrl);
                    }
                    var imageUrl = "";
                    if (request.ImageUrl != null)
                    {
                        imageUrl = await _unitOfWork.File.FileUpload(request.ImageUrl, "images/projects");
                    }

                    projectInfo.Title = (request.Title == "" || request.Title == null) ? projectInfo.Title : request.Title;
                    projectInfo.Description = (request.Description == "" || request.Description == null) ? projectInfo.Description : request.Description;
                    projectInfo.Catagory = (request.Catagory == "" || request.Catagory == null) ? projectInfo.Catagory : request.Catagory;
                    projectInfo.ImageUrl = request.ImageUrl == null ? projectInfo.ImageUrl : imageUrl;
                    projectInfo.LiveLink = (request.LiveLink == "" || request.LiveLink == null) ? projectInfo.LiveLink : request.LiveLink;
                    projectInfo.GithubLink = (request.GithubLink == "" || request.GithubLink == null) ? projectInfo.GithubLink : request.GithubLink;
                    projectInfo.Active = request.Active;

                    _unitOfWork.Project.Update(projectInfo);
                }
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Project updated successfully";
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
        public async Task<ApiResponse> DeleteProject(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Valid id required";
                return response;
            }
            var req = new GenericRequest<Project>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var projectInfo = await _unitOfWork.Project.GetAsync(req);
                _unitOfWork.Project.Remove(projectInfo);
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Project deleted successfully";
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