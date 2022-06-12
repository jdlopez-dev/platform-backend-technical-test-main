using AcmeCorporation.Core.Entities;
using AcmeCorporationApi.Models;
using AutoMapper;

namespace AcmeCorporationApi.Mappers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Person, PersonModel>().ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType.ToString()));
            CreateMap<PersonModel, Person>();
            CreateMap<PersonSaveModel, Person>();
        }
    }
}