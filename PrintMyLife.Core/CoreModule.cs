﻿using Autofac;
using System;
using System.Reflection;
using PrintMyLife.Core.Common.Dependencies;

namespace PrintMyLife.Core
{
    public class CoreModule : Autofac.Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			var core = typeof(CoreModule).GetTypeInfo().Assembly;
			builder.RegisterAssemblyTypes(core)
				   .Where(t => typeof(IManager).IsAssignableFrom(t))
				   .AsSelf()
				   .InstancePerLifetimeScope();
		}
	}
}
