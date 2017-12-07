using Autofac;
using EF = Microsoft.EntityFrameworkCore;
using System.Reflection;
using PrintMyLife.Infrastructure.EntityFramework;
using PrintMyLife.Common.Repositories;
using PrintMyLife.Common.UnitOWork;
using PrintMyLife.Infrastructure.Facebook;
using PrintMyLife.Core.Authentication;
using AutoMapper;

namespace PrintMyLife.Infrastructure
{
  public class InfrastructureModule : Autofac.Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
             .Where(t => t.Name.EndsWith("Service"))
             .AsImplementedInterfaces();

      builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
      builder.RegisterGeneric(typeof(Repository<,>)).As(typeof(IRepository<,>));
      builder.RegisterType<UnitOfWork<AppDbContext>>().As<IUnitOfWork>().InstancePerLifetimeScope();
      builder.RegisterType<AppDbContext>().As<EF.DbContext>().InstancePerLifetimeScope();
      
      builder.RegisterType<FacebookService>().As<IExternalSocialService>();

      builder.Register(ctx => new MapperConfiguration(cfg =>
      {
        cfg.AddProfile(new FacebookProfile());
      }));
      builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper()).As<IMapper>();
    }
  }
}
