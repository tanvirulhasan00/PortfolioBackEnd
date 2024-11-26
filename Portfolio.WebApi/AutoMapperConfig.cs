
using Portfolio.Models.DbModels;
using Portfolio.Models.RequestModels.PersonReqDto;
using AutoMapper;
using Portfolio.Models.RequestModels.ServiceReqDto;
using Portfolio.Models.RequestModels.EducationReqDto;
using Portfolio.Models.RequestModels.ExperienceReqDto;

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