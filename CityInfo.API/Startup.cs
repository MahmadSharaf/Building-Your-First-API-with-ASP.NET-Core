using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;

namespace CityInfo.API
{   //The startup class is the entry point of the application
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        // To work with settings in ASP.NET core app, we should instantiate a configuration here in the startup.
        // This configuration adheres to IConfigurationRoot interface, so one instance and application wide
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {   //construct the configuration 
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath) //That refers to content root of our environment
                .AddJsonFile ( "appSettings.json"                        , optional : false , reloadOnChange : true)  //This loads the appsettings.json in case of development
                .AddJsonFile ( $"appSettings.{env.EnvironmentName}.json" , optional : true  , reloadOnChange : true ) //This loads the appsettings.production.json in case of production  
                .AddEnvironmentVariables(); // This loads the environment variables. Environment variables is available the properties which include the fake connection string
            Configuration = builder.Build(); //Create configuration and store it in Configuration variable
        }

        //todo **************************************************************************************************
        //todo ***********************************ConfigureServices**********************************************
        //todo **************************************************************************************************
        // This is used to add services to the container to configure those services.
        //! A service is the component that intended for common consumption in an app
        //! There are frame work services, like identity, MVC, EF core services 
        //! There are application services
        //! There are built-in services, like the applicationBuilder and logger.
        //! -- The Logger is a built-in service but it needs to be configured 
        //!    in such that it knows what to log and where it should log it to*/

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                //Serialization: the property names in the response will be the same casing as set in the controller
                .AddJsonOptions(o => {
                    if (o.SerializerSettings.ContractResolver != null)
                    {
                        var castedResolver = o.SerializerSettings.ContractResolver
                            as DefaultContractResolver;
                        castedResolver.NamingStrategy = null;
                    }
                })

            //todo************* Add  Formatters   *********************!//
                // Formatting allows to configure the supported formatters for the API
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter())); //This adds XML support
            //todo///////////////////////////////////////////////////////

            //! Adding a custom service has the three types, which are different in their life time.
            // AddTransient: Its life time is created each time they are created each time they are requested. Best for lightweight stateless service.
            // AddScoped: are created once per request
            // AddSingleton: are created the first time they are requested or when the instance of configureServices is run. 

            //todo*************   Mail Service   *********************!//
            // Mail service is a light weight and stateless service 
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            //todo///////////////////////////////////////////////////////

            //todo*************   DB Migration   *********************!//
            //Hard-coded connectionString
            //x var connectionString = server=(localdb)\mssqllocaldb;Database=CityInfoDB;Trusted_Connection=True;"; //Connection string
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"];

            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString)); //Injecting DB context in startup services
                                                                                           // This now registered with our DB and connected
                                                                                           //todo///////////////////////////////////////////////////////

            services.AddScoped<ICityInfoRepository, CityInfoRepository>();
        }

        //todo ******************************************************************************************
        //todo ***********************************Configure**********************************************
        //todo ******************************************************************************************
        //! This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /* It uses services that are registered and configured in that method. It's used to specify how an ASP.NET
           Core application will respond to individual HTTP requests */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            CityInfoContext cityInfoContext) //Add new input parameter to be accepted by dependency injection container
        {
            //todo*************   L o g g e r   *********************!//
            // The below line added by default in .NET 2
            //xloggerFactory.AddConsole(); // This log into the console window

            // The below line added by default in .NET 2
            //x loggerFactory.AddDebug(); // This logs to debug window

            // This configures where the logger logs to
            //x loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider()); //
            // The above line has the below shortcut
            loggerFactory.AddNLog();
            //todo///////////////////////////////////////////////////////

            //todo*************  Exceptions Handler  *********************!//
            //! The exception handler middleware can potentially catch exceptions before
            //! handing the request over to MVC and, more importantly handle exceptions
            //! and returns response when an exception happens in the MVC-related code.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
            }
            //todo////////////////////////////////////////////////////

            //todo****************  Seed Data  *********************!//
            cityInfoContext.EnsureSeedDataForContext();
            //todo////////////////////////////////////////////////////

            //todo****************  Status codes  *********************!//
            //! To show the status code as a text on the web page
            app.UseStatusCodePages();
            //todo////////////////////////////////////////////////////

            //todo*************  M   V   C   *********************!//
            //! Added to the request pipeline, after exceptions are handled. So then
            //! MVC will handle http requests
            app.UseMvc();
            //todo////////////////////////////////////////////////////


            //todo ************** Unneeded code ******************//
            //xapp.Run(async (context) =>
            //x{
            //x    throw new Exception("EXample exception");

            //x    //await context.Response.WriteAsync("Hello World!");
            //x});
            //todo////////////////////////////////////////////////////
        }
    }
}
