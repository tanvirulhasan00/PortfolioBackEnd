
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.ApiRequestModels.GenericRequestModels;
using Portfolio.Models.ApiRequestModels.PersonReqDto;
using Portfolio.Models.ApiResponseModels.ApiResponse;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/person")]
    [ApiVersion("1.0")]
    public class PersonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ApiResponse response;
        public PersonController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.response = new ApiResponse();
        }
        [HttpGet]
        [Route("GetAll")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> GetAllPerson(CancellationToken cancellationToken)
        {
            var req = new GenericRequest<Person>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var person = await _unitOfWork.Person.GetAllAsync(req);
                if (person == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NoContent;
                    response.Message = "Unsuccessful";
                    return response;
                }
                response.Success = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Message = "Successful";
                response.Result = person;
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
        [Route("Create")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> CreatePerson([FromBody] PersonCreateDto request)
        {
            var isUniqueUser = _unitOfWork.Person.IsUniqueUser();
            if (!isUniqueUser)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.Ambiguous;
                response.Message = "Unsuccessful - User already exists.";
                return response;
            }

            var model = _mapper.Map<Person>(request);
            try
            {
                await _unitOfWork.Person.AddAsync(model);
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
                    response.Message = $"Unsuccessful";
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
        [Route("Update")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> UpdatePerson(int Id, [FromBody] PersonUpdateDto request, CancellationToken cancellationToken)
        {
            if (request == null || Id != 1)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Unsuccessful";
                return response;
            }
            var req = new GenericRequest<Person>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var personInfo = await _unitOfWork.Person.GetAsync(req);
                if (personInfo != null)
                {
                    personInfo.FirstName = (request.FirstName == "" || request.FirstName == null) ? personInfo.FirstName : request.FirstName;
                    personInfo.LastName = (request.LastName == "" || request.LastName == null) ? personInfo.LastName : request.LastName;
                    personInfo.PhoneNumber = (request.PhoneNumber == "" || request.PhoneNumber == null) ? personInfo.PhoneNumber : request.PhoneNumber;
                    personInfo.Email = (request.Email == "" || request.Email == null) ? personInfo.Email : request.Email;
                    personInfo.Bio = (request.Bio == "" || request.Bio == null) ? personInfo.Bio : request.Bio;
                    personInfo.Designation = (request.Designation == "" || request.Designation == null) ? personInfo.Designation : request.Designation;
                    personInfo.Address = (request.Address == "" || request.Address == null) ? personInfo.Address : request.Address;
                    personInfo.Country = (request.Country == "" || request.Country == null) ? personInfo.Country : request.Country;
                    personInfo.Nationality = (request.Nationality == "" || request.Nationality == null) ? personInfo.Nationality : request.Nationality;
                    personInfo.PostCode = (request.PostCode == "" || request.PostCode == null) ? personInfo.PostCode : request.PostCode;
                    personInfo.IsAvailableInFreelance = (request.IsAvailableInFreelance == "" || request.IsAvailableInFreelance == null) ? personInfo.IsAvailableInFreelance : request.IsAvailableInFreelance;
                    personInfo.NumberOfYearsOfExperience = request.NumberOfYearsOfExperience == 0 ? personInfo.NumberOfYearsOfExperience : request.NumberOfYearsOfExperience;
                    personInfo.NumberOfProjects = request.NumberOfProjects == 0 ? personInfo.NumberOfProjects : request.NumberOfProjects;
                    personInfo.NumberOfCodeCommits = request.NumberOfCodeCommits == 0 ? personInfo.NumberOfCodeCommits : request.NumberOfCodeCommits;
                    personInfo.NumberOfTechnologies = request.NumberOfTechnologies == 0 ? personInfo.NumberOfTechnologies : request.NumberOfTechnologies;
                    personInfo.ProfileImageUrl = (request.ProfileImageUrl == "" || request.ProfileImageUrl == null) ? personInfo.ProfileImageUrl : request.ProfileImageUrl;
                    personInfo.LogoUrl = (request.LogoUrl == "" || request.LogoUrl == null) ? personInfo.LogoUrl : request.LogoUrl;
                    personInfo.LinkedInLink = (request.LinkedInLink == "" || request.LinkedInLink == null) ? personInfo.LinkedInLink : request.LinkedInLink;
                    personInfo.GitHubLink = (request.GitHubLink == "" || request.GitHubLink == null) ? personInfo.GitHubLink : request.GitHubLink;
                    personInfo.YoutubeLink = (request.YoutubeLink == "" || request.YoutubeLink == null) ? personInfo.YoutubeLink : request.YoutubeLink;
                    personInfo.TwitterLink = (request.TwitterLink == "" || request.TwitterLink == null) ? personInfo.TwitterLink : request.TwitterLink;
                    personInfo.VideoCVLink = (request.VideoCVLink == "" || request.VideoCVLink == null) ? personInfo.VideoCVLink : request.VideoCVLink;

                    _unitOfWork.Person.Update(personInfo);
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
                    response.Message = "Unsuccessful";
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
        [Route("Remove")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> RemovePerson(int Id, CancellationToken cancellationToken)
        {
            var req = new GenericRequest<Person>
            {
                Expression = x => x.Id == Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            var personInfo = await _unitOfWork.Person.GetAsync(req);
            _unitOfWork.Person.Remove(personInfo);
            var result = await _unitOfWork.Save();
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
                response.Message = "Unsuccessful";
            }
            return response;
        }
    }
}