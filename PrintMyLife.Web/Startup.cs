using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using PrintMyLife.Core;
using PrintMyLife.Infrastructure;
using PrintMyLife.Infrastructure.EntityFramework;
using AutoMapper;
using PrintMyLife.Web.Controllers.Samples;
using PrintMyLife.Core.Authentication.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Serilog;
using PrintMyLife.Web.Controllers.Users;
using PrintMyLife.Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using PrintMyLife.Web.Runtime;
using PrintMyLife.Core.Runtime.Session;
using Microsoft.AspNetCore.Http;
using PrintMyLife.Web.Common.Filters;

public class Startup
{
  private readonly IHostingEnvironment _hostingEnvironment;

  public Startup(IHostingEnvironment env)
  {
    _hostingEnvironment = env;

    var builder = new ConfigurationBuilder()
        .SetBasePath(env.ContentRootPath)
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        .AddEnvironmentVariables();
    Configuration = builder.Build();

    Log.Logger = new LoggerConfiguration()
        .WriteTo.RollingFile(pathFormat: "logs\\log-web-{Date}.log")
        .CreateLogger();
  }

  public IConfigurationRoot Configuration { get; }

  public IContainer ApplicationContainer { get; private set; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public IServiceProvider ConfigureServices(IServiceCollection services)
  {
    //DataBase
    services.AddDbContext<AppDbContext>(options =>
      options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
    );

    //Identity
    services.AddIdentity<User, Role>()
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders()
      .AddUserStore<UserStore<User, Role, AppDbContext, Guid>>()
      .AddRoleStore<RoleStore<Role, AppDbContext, Guid>>();

    // Add framework services.
    services.AddMvc(options =>
    {
      options.Filters.AddService(typeof(ApiExceptionFilter));
    });

    //Swagger-ui 
    services.AddSwaggerGen(c =>
    {
      c.SwaggerDoc("v1", new Info
      {
        Title = "Test 1",
        Version = "v1",
        Description = "Welcome to the marvellous PrintMyLife API!"
      });
      c.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
    });

    //App settings
    services.Configure<AuthenticationSettings>(Configuration.GetSection("Authentication"));

    //Authentication
    AddJwtBearerAuthentication(services);

    //HttpContext
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

    //Automapper
    services.AddAutoMapper(cfg =>
    {
      cfg.AddProfile(new SamplesProfile());
      cfg.AddProfile(new UsersProfile());
    });

    // Autofac
    var builder = new ContainerBuilder();
    builder.RegisterModule(new CoreModule());
    builder.RegisterModule(new InfrastructureModule());
    builder.RegisterType<AppSession>().As<IAppSession>().InstancePerLifetimeScope();
    builder.RegisterType<ApiExceptionFilter>();
    builder.Populate(services);
    ApplicationContainer = builder.Build();

    return new AutofacServiceProvider(this.ApplicationContainer);
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
  {
    //Log
    loggerFactory.AddConsole(Configuration.GetSection("Logging"));
    loggerFactory.AddDebug();

    // only enable webpack building in Developement environment
    if (env.IsDevelopment())
    {
      //Swagger-ui
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PrintMyLife V1");
      });

      app.UseDeveloperExceptionPage();
      app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
      {
        HotModuleReplacement = true
      });
    }
    app.UseAuthentication();

    app.UseStaticFiles();

    app.UseMvc(routes =>
    {
      routes.MapRoute(
          name: "default",
          template: "api/{controller}/{id?}");
        // add a special route for our index page
        routes.MapSpaFallbackRoute(
              name: "spa-fallback",
              defaults: new { controller = "Home", action = "index" });
    });
  }

  public void AddJwtBearerAuthentication (IServiceCollection services)
  {
    var tokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = GetSignInKey(),
      ValidateIssuer = true,
      ValidIssuer = Configuration["Authentication:Issuer"],
      ValidateAudience = true,
      ValidAudience = Configuration["Authentication:Audience"],
      ValidateLifetime = true,
      ClockSkew = TimeSpan.Zero,
      RequireExpirationTime = true
    };

    services.AddAuthentication(options =>
    {
      options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
      o.TokenValidationParameters = tokenValidationParameters;
      o.RequireHttpsMetadata = false;
      o.SaveToken = true;
    });

    SecurityKey GetSignInKey()
    {
      var key = Encoding.ASCII.GetBytes(Configuration["Authentication:SecretKey"]);
      return new SymmetricSecurityKey(key);
    }
  }
}
