using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CityInfo.API
{
    public class Program
    {
        /* This for ASP.Net Core v1.0
        public static void Main(string[] args)
        {
            //This below block is the above line but in details
            var host = new WebHostBuilder() // This creates an instance of a host for a web app 
                                            //ASP.Net is decoupled from the web server environment that hosts the application
                                            //It ships with two different HTTP servers--WebListner which is a windows only web server and
                                            //Kestrel a cross-platform web server. Kestrel is the default.
                .UseKestrel()
                // UseContentRoot specifies the content root directory used by the web host.
                //The content root is the base path to any content used by the app,
                //such as its views and its web content.
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration() // This line signifies that IIS Express is used as a reverse proxy server for Kestrel
                                     // Kestrel can host the application alone but IIS gives more advantages 
                                    // such as: filter requests, manage DSL layer and certificates, make sure the application restarts if it crashes.
                .UseStartup<Startup>() // This specifies the startup type to be used by the web host.
                .Build(); // the builds an IWebHost instance to host our web application. 
                          // With that the webhost is configured and returned an IWebHost implementing instance
            host.Run(); // This will run the web application and it blocks the calling thread until the host shutdown
        }*/

        // This for ASP.NET Core v2 the same as for v1 but with extra featues 
        // Default files and variables for configurational setup as 
        public static void Main(string[] args) //This is responsible for running and configuring the application
        {
            
            CreateWebHostBuilder(args).Build().Run(); // This one calls CreateDefaultBuilder on an IWebHost instance 
        }   

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();


        
    

    }
}
