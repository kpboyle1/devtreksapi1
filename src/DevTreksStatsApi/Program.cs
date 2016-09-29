using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
//this package was added for the linux config
using Microsoft.Extensions.Configuration;

namespace DevTreks.DevTreksStatsApi
{
    public class Program
    {
        //iis windows servers
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        //linux web servers
        //public static void Main(string[] args)
        //{
        //    var config = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("hosting.json", optional: true)
        //        .Build();

        //    var host = new WebHostBuilder()
        //        .UseKestrel()
        //        .UseConfiguration(config)
        //        .UseContentRoot(Directory.GetCurrentDirectory())
        //        .UseStartup<Startup>()
        //        .Build();

        //    host.Run();
        //}

    }
}
