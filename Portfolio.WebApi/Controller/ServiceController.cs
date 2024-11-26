using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.GenericRequestModels;
using Portfolio.Models.RequestModels.PersonReqDto;
using Portfolio.Models.RequestModels.ServiceReqDto;
using Portfolio.Models.ResponseModels.ApiResponseModels;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.WebApi.Controller
{
    [ApiController]
    [Route("api/service")]
    public class ServiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse<Service> response;
        public ServiceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.response = new ApiResponse<Service>();
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<ApiResponse<Service>> GetAllService(CancellationToken cancellationToken)
        {
            var req = new GenericRequest<Service>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var service = await _unitOfWork.Service.GetAll(req);
                if (service == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.Error = new { message = "Unsuccessful" };
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Data = service;
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
        public async Task<ApiResponse<Service>> CreateService([FromBody] ServiceCreateDto request)
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
                var model = _mapper.Map<Service>(request);
                await _unitOfWork.Service.AddAsync(model);
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
        public async Task<ApiResponse<Service>> UpdateService(int Id, [FromBody] ServiceUpdateDto request, CancellationToken cancellationToken)
        {
            if (request == null || Id == 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }
            var req = new GenericRequest<Service>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var serviceInfo = await _unitOfWork.Service.Get(req);
                if (serviceInfo != null)
                {
                    serviceInfo.Name = (request.Name == "" || request.Name == null) ? serviceInfo.Name : request.Name;
                    serviceInfo.Description = (request.Description == "" || request.Description == null) ? serviceInfo.Description : request.Description;
                    serviceInfo.IsActive = request.IsActive;

                    _unitOfWork.Service.Update(serviceInfo);
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
        public async Task<ApiResponse<Service>> RemoveService(int Id, CancellationToken cancellationToken)
        {
            if (Id == 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Error = new { message = "Unsuccessful" };
                return response;
            }
            var req = new GenericRequest<Service>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var serivecInfo = await _unitOfWork.Service.Get(req);
                _unitOfWork.Service.Remove(serivecInfo);
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