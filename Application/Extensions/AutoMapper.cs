using Application.Command;
using AutoMapper;
using Domain.Dto;

namespace Application.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterUserCommand, UserRegisterDto>().ReverseMap();
        }
    }
}
