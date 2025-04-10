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

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/customer-message")]
    [ApiVersion("1.0")]
    public class CustomerMessageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse response;

        public CustomerMessageController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            response = new ApiResponse();
        }
        [HttpGet]
        [Route("getall")]
        public async Task<ApiResponse> GetAllCustomerMessage(CancellationToken cancellationToken)
        {
            var req = new GenericRequest<CustomerMessage>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            var projects = await _unitOfWork.CustomerMessage.GetAllAsync(req);
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
        public async Task<ApiResponse> GetCustomerMessage(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Valid id required";
                return response;
            }
            var req = new GenericRequest<CustomerMessage>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var projects = await _unitOfWork.CustomerMessage.GetAsync(req);
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
        public async Task<ApiResponse> CreateCustomerMessage(CustomerMessageCreateDto request)
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
                CustomerMessage customerMessage = new()
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    ServiceName = request.ServiceName,
                    Message = request.Message,
                };

                await _unitOfWork.CustomerMessage.AddAsync(customerMessage);
                int result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Customer-Message created successfully";
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
        public async Task<ApiResponse> DeleteCustomerMessage(int Id, CancellationToken cancellationToken)
        {
            if (Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Valid id required";
                return response;
            }
            var req = new GenericRequest<CustomerMessage>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var customerMessage = await _unitOfWork.CustomerMessage.GetAsync(req);
                _unitOfWork.CustomerMessage.Remove(customerMessage);
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Customer-Message deleted successfully";
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