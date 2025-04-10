
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
        [Route("getall")]
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
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "Data not found.";
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
        [Route("update")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> UpdatePerson(PersonUpdateDto request, CancellationToken cancellationToken)
        {
            if (request.Id <= 0)
            {
                response.Success = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = "Id required.";
                return response;
            }
            var req = new GenericRequest<Person>
            {
                Expression = x => x.Id == request.Id,
                IncludeProperties = null,
                NoTracking = true,
                CancellationToken = cancellationToken
            };
            try
            {
                var personInfo = await _unitOfWork.Person.GetAsync(req);
                if (personInfo == null)
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = $"data not found with the id {request.Id}";
                    return response;
                }
                if (personInfo != null)
                {
                    if (!string.IsNullOrEmpty(personInfo.ProfileImageUrl))
                    {
                        _unitOfWork.File.DeleteFile(personInfo.ProfileImageUrl);
                    }
                    if (!string.IsNullOrEmpty(personInfo.LogoUrl))
                    {
                        _unitOfWork.File.DeleteFile(personInfo.LogoUrl);
                    }
                    if (!string.IsNullOrEmpty(personInfo.CVDownloadLink))
                    {
                        _unitOfWork.File.DeleteFile(personInfo.CVDownloadLink);
                    }
                    if (!string.IsNullOrEmpty(personInfo.VideoCVLink))
                    {
                        _unitOfWork.File.DeleteFile(personInfo.VideoCVLink);
                    }
                    var profilePicUrl = "";
                    var logoUrl = "";
                    var cvUrl = "";
                    var videoCvUrl = "";
                    if (request.ProfileImageUrl != null)
                    {
                        profilePicUrl = await _unitOfWork.File.FileUpload(request.ProfileImageUrl, "images");
                    }
                    if (request.LogoUrl != null)
                    {
                        logoUrl = await _unitOfWork.File.FileUpload(request.LogoUrl, "images");
                    }
                    if (request.CVDownloadLink != null)
                    {
                        cvUrl = await _unitOfWork.File.FileUpload(request.CVDownloadLink, "pdfs");
                    }
                    if (request.VideoCVLink != null)
                    {
                        videoCvUrl = await _unitOfWork.File.FileUpload(request.VideoCVLink, "videos");
                    }
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
                    personInfo.AvailableInFreelance = (request.AvailableInFreelance == 0) ? personInfo.AvailableInFreelance : request.AvailableInFreelance;
                    personInfo.NumberOfYearsOfExperience = request.NumberOfYearsOfExperience == 0 ? personInfo.NumberOfYearsOfExperience : request.NumberOfYearsOfExperience;
                    personInfo.NumberOfProjects = request.NumberOfProjects == 0 ? personInfo.NumberOfProjects : request.NumberOfProjects;
                    personInfo.NumberOfCodeCommits = request.NumberOfCodeCommits == 0 ? personInfo.NumberOfCodeCommits : request.NumberOfCodeCommits;
                    personInfo.NumberOfTechnologies = request.NumberOfTechnologies == 0 ? personInfo.NumberOfTechnologies : request.NumberOfTechnologies;
                    personInfo.ProfileImageUrl = (request.ProfileImageUrl == null) ? personInfo.ProfileImageUrl : profilePicUrl;
                    personInfo.LogoUrl = (request.LogoUrl == null) ? personInfo.LogoUrl : logoUrl;
                    personInfo.LinkedInLink = (request.LinkedInLink == "" || request.LinkedInLink == null) ? personInfo.LinkedInLink : request.LinkedInLink;
                    personInfo.GitHubLink = (request.GitHubLink == "" || request.GitHubLink == null) ? personInfo.GitHubLink : request.GitHubLink;
                    personInfo.YoutubeLink = (request.YoutubeLink == "" || request.YoutubeLink == null) ? personInfo.YoutubeLink : request.YoutubeLink;
                    personInfo.TwitterLink = (request.TwitterLink == "" || request.TwitterLink == null) ? personInfo.TwitterLink : request.TwitterLink;
                    personInfo.CVDownloadLink = (request.CVDownloadLink == null) ? personInfo.CVDownloadLink : cvUrl;
                    personInfo.VideoCVLink = (request.VideoCVLink == null) ? personInfo.VideoCVLink : videoCvUrl;

                    _unitOfWork.Person.Update(personInfo);
                }
                var result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.Success = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = "Person updated successfully.";
                    return response;
                }
                else
                {
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "Update failed.";
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
        [Route("remove")]
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