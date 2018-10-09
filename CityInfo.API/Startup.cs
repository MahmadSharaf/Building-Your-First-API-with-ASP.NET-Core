using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CityInfo.API
{   //The startup class is the entry point of the application
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        /*This is used to add services to the container to configure those services.*/
        /* A service is the component that intended for common consumptiin in an app
         * There are frame work services, like identity, MVC, EF core services 
         * There are application services
         * There are built-in services, like the applicationBuilder and logger.*/
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /* It uses services that are registered and configured in that method. It's used to specify how an ASP.NET
           Core application will respond to individual HTTP requests */
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
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
