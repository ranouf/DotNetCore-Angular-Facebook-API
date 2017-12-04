using AutoMapper;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Web.Controllers.Authorization.Dto;
using PrintMyLife.Web.Controllers.Users.Dto;

namespace PrintMyLife.Web.Controllers.Authorization
{
  public class AuthorizationProfile : Profile
  {
    public AuthorizationProfile()
    {
      CreateMap<User, UserAuthenticationDto>();
    }
  }
}
