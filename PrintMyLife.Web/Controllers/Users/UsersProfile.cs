using AutoMapper;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Web.Controllers.Users.Dto;

namespace PrintMyLife.Web.Controllers.Users
{
  public class UsersProfile : Profile
  {
    public UsersProfile()
    {
      CreateMap<User, UserDto>();
    }
  }
}
