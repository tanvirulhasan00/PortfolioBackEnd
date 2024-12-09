using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.GenericRequestModels;
using Portfolio.Models.ResponseModels.ApiResponseModels;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.WebApi.Controller.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
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

    }
}