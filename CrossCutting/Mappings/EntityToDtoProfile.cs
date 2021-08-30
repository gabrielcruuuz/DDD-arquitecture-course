using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using AutoMapper;

namespace Api.CrossCutting.Mappings
{
    public class EntityToDtoProfile : Profile
    {
        public EntityToDtoProfile()
        {
            CreateMap<UserDto, UserEntity>()
             .ReverseMap();

            CreateMap<UserDtoCreate, UserEntity>()
                .ReverseMap();

            CreateMap<UserDtoUpdate, UserEntity>()
              .ReverseMap();

            CreateMap<UserDtoCreateResult, UserEntity>()
               .ReverseMap();

            CreateMap<UserDtoUpdateResult, UserEntity>()
            .ReverseMap();
        }
    }
}
