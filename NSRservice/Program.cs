using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace NSRservice
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            var i = configuration.GetSection("NSRsettings").GetChildren();
            Console.WriteLine(i);

            Console.WriteLine("Hello World!");
        }
    }
}
