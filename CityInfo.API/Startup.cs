using CityInfo.API.Entities;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace CityInfo.API
{   //The startup class is the entry point of the application
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath) //That refers to content root of our environment
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true) //Adds json file
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);  //Adds json file

            Configuration = builder.Build(); //Create configuration and store it in Configuration variable
        }
        
        /*This is used to add services to the container to configure those services.*/
        /* A service is the component that intended for common consumptiom in an app
         * There are frame work services, like identity, MVC, EF core services 
         * There are application services
         * There are built-in services, like the applicationBuilder and logger.
           -- The Logger is a built-in service but it needs to be configured 
              in such that it knows what to log and where it should log it to*/
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                // Formatting allows to configure the supported formatters for the API
                .AddMvcOptions(o => o.OutputFormatters.Add(
                    new XmlDataContractSerializerOutputFormatter())); //This adds XML support

            // Adding a custom service has the three types, which are different in their life time.
            // AddTransient: Its life time is created each time they are created each time they are requested. Best for lightweight stateless service.
            // AddScoped: are created once per request
            // AddSingleton: are created the first time they are requested or when the instance of configureServices is run. 
            // Mail service is a light weight and stateless service 
#if DEBUG
            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            var connectionString = @"server=(localdb)\mssqllocaldb;Database=CityInfoDB;Trusted_Connection=True;";
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString)); //Injecting DB context in startup services
                                                      // This now registered with our DB and connected

            //Serialization
            //.AddJsonOptions(o => {
            //     if (o.SerializerSettings.ContractResolver != null)
            //     {
            //         var castedResolver = o.SerializerSettings.ContractResolver
            //             as DefaultContractResolver;
            //         castedResolver.NamingStrategy = null;
            //     }
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /* It uses services that are registered and configured in that method. It's used to specify how an ASP.NET
           Core application will respond to individual HTTP requests */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // The below line added by default in .NET 2
            //loggerFactory.AddConsole(); // This log into the console window

            // The below line added by default in .NET 2
            // loggerFactory.AddDebug(); // This logs to debug window

            // This configures where the logger logs to
            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider()); //
            // The above line has the below shortcut
            loggerFactory.AddNLog(); 

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); //This catches exceptions then handles the requests over to MVC, in such that,
            }                                    // it returns the correct response when an exception happens in the MVC-related code

            app.UseStatusCodePages(); //To show the status code as a text on the web page

            // After exceptions are handled
            app.UseMvc(); // This requests pipelines

            //app.Run(async (context) =>
            //{
            //    throw new Exception("EXample exception");

            //    //await context.Response.WriteAsync("Hello World!");
            //});
        }
    }
}
