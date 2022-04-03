using System.Linq;
using AutoMapper;

namespace CRM.User.WebApp.Models.Basic.User.UserProfileDto
{
    public class UserProfileDtoProfile : Profile
    {
        public UserProfileDtoProfile()
        {
            CreateMap<DAL.Models.DatabaseModels.Users.User, UserProfileDto>()
                .ForMember(dest => dest.Roles,
                    opt => opt.MapFrom(src => src.UserRoles.Select(i => i.Role.Name)));
        }
    }
}