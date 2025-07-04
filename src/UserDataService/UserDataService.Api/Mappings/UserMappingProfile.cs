using AutoMapper;
using UserDataService.Domain.Aggregates.UserAggregate;
using UserDataService.Api.Commands.UserCommands.CreateCommands;
using UserDataService.Api.Commands.UserCommands.UpdateCommands;
using UserDataService.Infrastructure.Models.UserDTO;

namespace UserDataService.Api.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<CreateUserCommand, User>()
                .ForMember(u => u.Id, opt => opt.Ignore())
                .ForMember(u => u.CreatedAt, opt => opt.Ignore())
                .ForMember(u => u.IsDeleted, opt => opt.Ignore());

            CreateMap<UpdateUserCommand, User>()
                .ForMember(u => u.CreatedAt, opt => opt.Ignore())
                .ForMember(u => u.IsDeleted, opt => opt.Ignore());

            CreateMap<User, UserResponse>()
                .ForMember(ur => ur.PictureUri, opt => opt.Ignore());

            CreateMap<User, CreateUserResponse>()
                .ForMember(ur => ur.PictureUri, opt => opt.Ignore());

            CreateMap<User, UpdateUserResponse>()
                .ForMember(ur => ur.PictureUri, opt => opt.Ignore());
        }
    }
}
