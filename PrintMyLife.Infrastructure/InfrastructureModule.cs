using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using PrintMyLife.Core.Common.Repositories;
using PrintMyLife.Core.Common.UnitOWork;
using PrintMyLife.Core.Sample;
using PrintMyLife.Infrastructure.EntityFramework;
using PrintMyLife.Infrastructure.Sample;
using MyRepository = PrintMyLife.Core.Common.Repositories;
using MyUnitOWork = PrintMyLife.Core.Common.UnitOWork;

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
    }
  }
}
