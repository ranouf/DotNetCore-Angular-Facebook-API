using AutoMapper;
using PrintMyLife.Core.Sample.Entities;
using PrintMyLife.Web.Controllers.Samples.Dto;

namespace PrintMyLife.Web.Controllers.Samples
{
  public class SamplesProfile : Profile
  {
    public SamplesProfile()
    {
      CreateMap<MySample, MySampleDto>();
    }
  }
}
