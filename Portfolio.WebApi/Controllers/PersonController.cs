using System.Net;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Models.ApiRequestModels.Person;
using Portfolio.Models.ApiResponseModels.ApiResponse;
using Portfolio.Models.IRepositoryRequestModels;
using Portfolio.Models.PortfolioModels;
using Portfolio.RepositoryConfig.IRepositories;

namespace Portfolio.WebApi.Controllers
{
    [ApiController]
    [Route("api/Person")]
    public class PersonController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public PersonController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        [Route("GetAllPerson")]
        public async Task<ApiResponse<Person>> GetAllPerson(CancellationToken cancellationToken)
        {
            var response = new ApiResponse<Person>();
            var GenericRequest = new IRepositoryRequest<Person>
            {
                Expression = null,
                IncludeProperties = null,
                NoTracking = false,
                CancellationToken = cancellationToken
            };
            try
            {
                var person = await _unitOfWork.Person.GetAllAsync(GenericRequest);

                response.StatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
                response.Message = ["Successfull"];
                response.Result = person;
            }
            catch (OperationCanceledException ex)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Message = [$"{ex.Message}"];
            }
            return response;
        }

        //create Person if does not exists
        [HttpPost]
        [Route("CreatePerson")]
        public async Task<ApiResponse<PersonRequest>> CreatePerson([FromBody] PersonRequest person)
        {
            var response = new ApiResponse<PersonRequest>();
            if (person == null)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = ["Form data required."];
                return response;
            }
            var personData = new Person
            {
                FirstName = person.FirstName ?? "",
                LastName = person.LastName ?? "",
                Age = person.Age!,
                PhoneNumber = person.PhoneNumber ?? "",
                Email = person.Email ?? "",
                Designation = person.Designation ?? "",
                DateOfBirth = person.DateOfBirth,
                Address = person.Address ?? "",
                Country = person.Country ?? "",
                ZipCode = person.ZipCode ?? "",
                Bio = person.Bio ?? "",
                ProfileImageUrl = person.ProfileImageUrl ?? "",
                CoverImageUrl = person.CoverImageUrl ?? "",
                LinkedInLink = person.LinkedInLink ?? "",
                GitHubLink = person.GitHubLink ?? "",
                VideoLink = person.VideoLink ?? "",
            };
            bool uniqueUser = _unitOfWork.Person.IsUserUnique();
            if (uniqueUser)
            {
                await _unitOfWork.Person.AddAsync(personData);
                int result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = new List<string>() { "Person Created Successfully." };
                }
                else
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = new List<string>() { "Unsuccessfull" };
                }
            }
            else
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.Conflict;
                response.Message = ["User Already exits."];
            }

            return response;
        }

        // Update person
        [HttpPost]
        [Route("UpdatePerson")]
        public async Task<ApiResponse<Person>> UpdatePerson(int id, [FromBody] PersonRequest person, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<Person>();
            var GenericRequest = new IRepositoryRequest<Person>
            {
                Expression = x => x.PersonId == id,
                IncludeProperties = null,
                NoTracking = false,
                CancellationToken = cancellationToken,

            };
            int result = 0;
            if (id != 1)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = ["Id required."];
                return response;
            }
            var personData = await _unitOfWork.Person.GetAsync(GenericRequest);
            if (personData != null)
            {
                personData.FirstName = (person.FirstName == "" || person.FirstName == null) ? personData.FirstName : person.FirstName;
                personData.LastName = (person.LastName == "" || person.LastName == null) ? personData.LastName : person.LastName;
                personData.Age = person.Age == 0 ? personData.Age : person.Age;
                personData.PhoneNumber = (person.PhoneNumber == "" || person.PhoneNumber == null) ? personData.PhoneNumber : person.PhoneNumber;
                personData.Email = (person.Email == "" || person.Email == null) ? personData.Email : person.Email;
                personData.Designation = (person.Designation == "" || person.Designation == null) ? personData.Designation : person.Designation;
                personData.DateOfBirth = person.DateOfBirth;
                personData.Address = (person.Address == "" || person.Address == null) ? personData.Address : person.Address;
                personData.Country = (person.Country == "" || person.Country == null) ? personData.Country : person.Country;
                personData.ZipCode = (person.ZipCode == "" || person.ZipCode == null) ? personData.ZipCode : person.ZipCode;
                personData.Bio = (person.Bio == "" || person.Bio == null) ? personData.Bio : person.Bio;
                personData.ProfileImageUrl = (person.ProfileImageUrl == "" || person.ProfileImageUrl == null) ? personData.ProfileImageUrl : person.ProfileImageUrl;
                personData.CoverImageUrl = (person.CoverImageUrl == "" || person.CoverImageUrl == null) ? personData.CoverImageUrl : person.CoverImageUrl;
                personData.LinkedInLink = (person.LinkedInLink == "" || person.LinkedInLink == null) ? personData.LinkedInLink : person.LinkedInLink;
                personData.GitHubLink = (person.GitHubLink == "" || person.GitHubLink == null) ? personData.GitHubLink : person.GitHubLink;
                personData.VideoLink = (person.VideoLink == "" || person.VideoLink == null) ? personData.VideoLink : person.VideoLink;

                _unitOfWork.Person.Update(personData);
                result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = ["Update successfull."];
                    response.Result = personData;
                }
                else
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = ["Update Unsuccessfull."];
                }

            }
            return response;
        }

        //remove person
        [HttpPost]
        [Route("RemovePerson")]
        public async Task<ApiResponse<Person>> RemovePerson(int id, CancellationToken cancellationToken)
        {
            var response = new ApiResponse<Person>();
            var GenericRequest = new IRepositoryRequest<Person>
            {
                Expression = x => x.PersonId == id,
                IncludeProperties = null,
                NoTracking = false,
                CancellationToken = cancellationToken,
            };
            int result = 0;
            if (id != 1)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = ["Id required."];
            }
            var personData = await _unitOfWork.Person.GetAsync(GenericRequest);
            if (personData != null)
            {
                _unitOfWork.Person.Remove(personData);
                result = await _unitOfWork.Save();
                if (result > 0)
                {
                    response.IsSuccess = true;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = ["Successfully removed."];
                }
                else
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = ["remove request unsuccessfull."];
                }
            }
            return response;
        }
    }
}