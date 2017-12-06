using AutoMapper;
using PrintMyLife.Core.Authentication.Entities;
using PrintMyLife.Core.Social.Entities;
using PrintMyLife.Infrastructure.Facebook.Converters;
using PrintMyLife.Infrastructure.Facebook.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrintMyLife.Infrastructure.Facebook
{
  public class FacebookProfile : Profile
  {
    public FacebookProfile()
    {
      CreateMap<AccessTokenResponse, AccessToken>();
      CreateMap<UserProfileResponse, UserProfile>()
        .ForMember(
            dest => dest.FullName,
            opts => opts.MapFrom(src => src.Name)
        )
        .ForMember(
            dest => dest.CoverUrl,
            opts => opts.MapFrom(src => src.Cover.Source)
        );
      CreateMap<DebugTokenResponse, UserToken>()
        .ForMember(
            dest => dest.AppId,
            opts => opts.MapFrom(src => src.Data.AppId)
        )
        .ForMember(
            dest => dest.Application,
            opts => opts.MapFrom(src => src.Data.Application)
        )
        .ForMember(
            dest => dest.ExpiresAt,
            opts => opts.MapFrom(src => src.Data.ExpiresAt)
        )
        .ForMember(
            dest => dest.IssuedAt,
            opts => opts.MapFrom(src => src.Data.IssuedAt)
        )
        .ForMember(
            dest => dest.IsValid,
            opts => opts.MapFrom(src => src.Data.IsValid)
        )
        .ForMember(
            dest => dest.Scopes,
            opts => opts.MapFrom(src => src.Data.Scopes)
        )
        .ForMember(
            dest => dest.Type,
            opts => opts.MapFrom(src => src.Data.Type)
        )
        .ForMember(
            dest => dest.UserId,
            opts => opts.MapFrom(src => src.Data.UserId)
        );

      CreateMap<AccountsResponse, IEnumerable<Account>>()
        .ConvertUsing<AccountsConverter>();
      CreateMap<Datum, Account>()
        .ForMember(
            dest => dest.CoverUrl,
            opts => opts.MapFrom(src => src.Cover.Source)
        );
    }
  }
}
