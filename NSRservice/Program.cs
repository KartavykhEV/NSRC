using System;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Threading;
using Microsoft.Extensions.Configuration;
using NSRcommon;

namespace NSRservice
{
    class Program
    {
        static internal String serverName;
        static internal String host;
        static internal int port;


        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            serverName = config["name"];
            host = config["host"]; port = Convert.ToInt32(config["port"]);

            callConnect();

            Console.WriteLine(serverName);
            Thread lth = new Thread(internalThread);
            lth.Start();
            lth.Join();


        }

        static int small_timeout = 2,   // частый интервал при автивности
            timeout = small_timeout,    // интервал запросов к серверу
            big_timeout = 1 * 60;       // большой интервал 

        static void internalThread()
        {
            WriteConsole("Запуск сервиса NSRservice");
            int cnt = 0, max_cnt = 5 * 60 / small_timeout;
            while (true)
            {
                Boolean rv = callConnect();
                Thread.Sleep(1000 * timeout);
                cnt++;
                if (rv) cnt = 0; // была команда - сбрасываем счетчич перехода на большой интервал
                if (timeout == small_timeout && cnt > max_cnt)
                    timeout = big_timeout;                      // переключаем на большой таймаут
            }
        }



        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static NSRCresponse executeCommand(NSRCcommand command)
        {
            return new NSRCresponse(command.Id)
            {
                Success = true,
                Response = runBash(command.CommandText),
            };
            //else return new NSRCresponse(command.Id) { Success = false, Response = "Command not supported" };
        }

        /// <summary>
        /// Выполнение команды в shell
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string runBash(string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            WriteConsole($"executing: {cmd}");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }

        /// <summary>
        /// Запрос на сервер
        /// </summary>
        /// <returns></returns>
        static Boolean callConnect()
        {
            BasicHttpsBinding basicHttpBinding = null;
            EndpointAddress endpointAddress = null;
            ChannelFactory<INSRCservice> factory = null;
            INSRCservice serviceProxy = null;

            Boolean rv = false;

            try
            {
                basicHttpBinding = new BasicHttpsBinding();
                basicHttpBinding.Security.Mode = BasicHttpsSecurityMode.Transport;
                endpointAddress = new EndpointAddress(new Uri($"https://{host}:{port}/nsrc_service.asmx"));
                factory = new ChannelFactory<INSRCservice>(basicHttpBinding, endpointAddress);
                serviceProxy = factory.CreateChannel();

                NSRCcommand command = serviceProxy.Connect(serverName);
                while (command != null)
                {
                    rv = true;
                    NSRCresponse response = executeCommand(command);
                    command = serviceProxy.SendResponse(serverName, response);
                    timeout = 2; // переключаем в режим частой проверки
                }
                WriteConsole("command null");

                factory.Close();
                ((ICommunicationObject)serviceProxy).Close();
            }
            catch (Exception ex)
            {
                WriteConsole(ex.Message, true);
            }
            return rv;
        }

        static void WriteConsole(String text, Boolean error = false) => Console.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} {(error ? "[e]" : "[i]")} {text}");



    }
}
