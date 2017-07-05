using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography.X509Certificates;
using System.Runtime;






namespace Core
{
    public class Program
    {
        public static void Main(string[] args)
        {   
            //Environment.SetEnvironmentVariable("DEVHOST","WINDOWSONMAC");
            if (Environment.GetEnvironmentVariable("DEVHOST")==null){
                 var host = new WebHostBuilder()
                .UseUrls("https://www.uberduber.com")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
                host.Run();
            }else{
                var cert = new X509Certificate2("mypfx.pfx", "a2bman");
                var host = new WebHostBuilder()
                    .UseUrls("https://www.uberduber.net/", "http://www.uberduber.net/")  //host.Start();
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseKestrel(cfg => cfg.UseHttps(cert))
                    .UseIISIntegration()
                    .UseStartup<Startup>()
                    .Build();
                host.Run();
            }
        }
    }
}
