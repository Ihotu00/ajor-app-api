using AjorApi.Models.Dtos;
using AutoMapper;

namespace AjorApi.Models.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Organization, CreateOrganizationDto>().ReverseMap();
            CreateMap<Organization, GetOrganizationsDto>().ReverseMap();
            CreateMap<Users, CreateUsersDto>().ReverseMap();
            CreateMap<Users, GetUsersDto>().ReverseMap();
            CreateMap<Users, EditUsersDto>().ReverseMap();
            CreateMap<Contribution, GetContributionDto>().ReverseMap();
            CreateMap<Contribution, CreateContributionDto>().ReverseMap();
            CreateMap<Contributor, CreateContributorDto>().ReverseMap();
            CreateMap<Contributor, GetContributorDto>().ReverseMap();
            CreateMap<Contributor, GetContributorsOfAContributionDto>().ReverseMap();
        }
    }
}