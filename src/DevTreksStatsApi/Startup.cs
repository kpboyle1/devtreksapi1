using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DevTreks.DevTreksStatsApi.Models;

namespace DevTreks.DevTreksStatsApi
{
    /// <summary>
    ///Purpose:		Configure the web app and start MVC webapi page
    ///             delivery.
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	www.devtreks.org
    /// </summary>
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        private static string DefaultRootFullFilePath { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            //set the webroot full file path: C:\\DevTreks\\src\\DevTreks\\wwwroot
            DefaultRootFullFilePath = string.Concat(env.WebRootPath, "\\");
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            bool bIsDevelopment = true;
            // Add framework services.
            services.AddMvc();
            //di the repository into the controller
            //this sets the 2 paths to script executables (RScript.exe and pythonw.exe) and the isdevelopment param
            services.AddSingleton<IStatScriptRepository>(new StatScriptRepository("statscript"
                , string.Empty
                , string.Empty
                , string.Empty
                , Configuration["Site:PyExecutable"]
                , Configuration["Site:RExecutable"]
                , DefaultRootFullFilePath
                , Configuration["Debug:DefaultRootWebStoragePath"]
                , bIsDevelopment));
        }
        public void ConfigureProductionServices(IServiceCollection services)
        {
            bool bIsDevelopment = false;
            // Add framework services.
            services.AddMvc();
            //di the repository into the controller
            //this sets the 2 paths to script executables (RScript.exe and pythonw.exe)
            services.AddSingleton<IStatScriptRepository>(new StatScriptRepository("statscript"
                , string.Empty
                , string.Empty
                , string.Empty
                , Configuration["Site:PyExecutable"]
                , Configuration["Site:RExecutable"]
                , DefaultRootFullFilePath
                , Configuration["Debug:DefaultRootWebStoragePath"]
                , bIsDevelopment));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
