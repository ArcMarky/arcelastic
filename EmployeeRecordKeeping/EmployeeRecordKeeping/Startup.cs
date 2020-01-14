using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using EmployeeRecordKeeping.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace EmployeeRecordKeeping
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                //builder.AddUserSecrets();
            }
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            var databaseConnectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<rksContext>(options =>
            options.UseMySQL(databaseConnectionString));
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    builder.AllowAnyOrigin()//TODO: add production urls here
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
            services.AddAutoMapper(typeof(Startup));
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            //auto discovery of the services and interfaces
            // Create the container builder.
            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);
            containerBuilder.RegisterType<Startup>().As<Startup>();

            //Services Registration
            List<Type> servicesList = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "EmployeeRecordKeeping.BLL.Services").Where(type => type.IsClass && !type.IsAbstract && !type.IsGenericType && !type.IsNested).ToList<Type>();
            foreach (var type in servicesList)
            {
                Type typeInterface = type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault();
                containerBuilder.RegisterType(type).As(typeInterface);
            }

            //Repositories Registration
            List<Type> repositoriesList = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "EmployeeRecordKeeping.DAL.Repositories").Where(type => type.IsClass && !type.IsAbstract && !type.IsGenericType && !type.IsNested).ToList<Type>();
            foreach (var type in repositoriesList)
            {
                Type typeInterface = type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault();
                containerBuilder.RegisterType(type).As(typeInterface);
            }

            var appContainer = containerBuilder.Build();
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(appContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
        private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }
    }
}
