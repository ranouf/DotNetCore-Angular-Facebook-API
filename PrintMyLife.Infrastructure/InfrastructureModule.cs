using Autofac;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using PrintMyLife.Core.Sample;
using PrintMyLife.Infrastructure.EntityFramework;
using PrintMyLife.Infrastructure.Sample;
using MyRepository = PrintMyLife.Core.Common.Repositories;
using MyUnitOWork = PrintMyLife.Core.Common.UnitOWork;
using PrintMyLife.Infrastructure.Facebook;
using PrintMyLife.Core.Authentication;
using AutoMapper;
using System.Collections.Generic;

namespace PrintMyLife.Infrastructure
{
  public class InfrastructureModule : Autofac.Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
             .Where(t => t.Name.EndsWith("Service"))
             .AsImplementedInterfaces();

      builder.RegisterGeneric(typeof(MyRepository.Repository<>)).As(typeof(MyRepository.IRepository<>));
      builder.RegisterGeneric(typeof(MyRepository.Repository<,>)).As(typeof(MyRepository.IRepository<,>));
      builder.RegisterType<MyUnitOWork.UnitOfWork<AppDbContext>>().As<MyUnitOWork.IUnitOfWork>().InstancePerLifetimeScope();
      builder.RegisterType<AppDbContext>().As<DbContext>().InstancePerLifetimeScope();

      builder.RegisterType<SampleService>().As<ISampleService>();
      builder.RegisterType<FacebookService>().As<IExternalSocialService>();

      builder.Register(ctx => new MapperConfiguration(cfg =>
      {
        cfg.AddProfile(new FacebookProfile());
      }));
      builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();
    }
  }
}
