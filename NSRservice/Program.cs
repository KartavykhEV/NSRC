﻿using System; 
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
            String serverName = configuration["Name"]; 
            Console.WriteLine(serverName); 

            Console.WriteLine("Hello World!"); 
        }
    }
}
