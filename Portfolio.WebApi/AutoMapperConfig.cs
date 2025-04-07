
using Portfolio.Models.PortfolioModels;
using Portfolio.Models.ApiRequestModels.PersonReqDto;
using AutoMapper;
using Portfolio.Models.ApiRequestModels.ServiceReqDto;
using Portfolio.Models.ApiRequestModels.EducationReqDto;
using Portfolio.Models.ApiRequestModels.ExperienceReqDto;

namespace Portfolio.WebApi
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            //CreateMap<Person, PersonDto>().ReverseMap();
            CreateMap<Person, PersonCreateDto>().ReverseMap();
            CreateMap<Person, PersonUpdateDto>().ReverseMap();

            CreateMap<Service, ServiceCreateDto>().ReverseMap();
            CreateMap<Service, ServiceUpdateDto>().ReverseMap();

            CreateMap<Education, EducationCreateDto>().ReverseMap();
            CreateMap<Education, EducationUpdateDto>().ReverseMap();

            CreateMap<Experience, ExperienceCreateDto>().ReverseMap();
            CreateMap<Experience, ExperienceUpdateDto>().ReverseMap();
        }
    }
}