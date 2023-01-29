using AutoMapper;
using ChatAPI.Application.RepositoryDTOs.AuthRepository;
using ChatAPI.Application.RepositoryDTOs.ChatRepository.GetListAllChats;
using ChatAPI.Application.RepositoryDTOs.MessageReposiory.SendMessage;
using ChatAPI.Application.RepositoryDTOs.UserRepository;
using ChatAPI.Domain.Entities;

namespace ChatAPI.Application.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, RegisterDTO.Request>().ReverseMap();
            CreateMap<User, GetUserDTO.Response>().ReverseMap();
            CreateMap<UpdateUserDTO.Request, User>().ReverseMap();
            CreateMap<Message, LastMessageDto>().ReverseMap();

            CreateMap<Message, SendMessageDto.Request>().ReverseMap();
            CreateMap<Message, SendMessageDto.Response>().ReverseMap();
        }
    }
}
