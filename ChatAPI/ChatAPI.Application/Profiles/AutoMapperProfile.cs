using AutoMapper;
using ChatAPI.Application.RepositoryDTOs.AuthRepository;
using ChatAPI.Domain.Entities;

namespace ChatAPI.Application.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, RegisterDTO.Request>().ReverseMap();

        }
    }
}
