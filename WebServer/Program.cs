using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using WebServer.API;
using WebServer.Model;

namespace WebServer
{
    class Program
    {

        static void Main(string[] args)
        {
            StartLogger();


            BaseServerModell baseServer = null;
            try
            {
                baseServer = new BaseServerModell(IPAddress.Parse(AppSettings.Settings.Host),AppSettings.Settings.Port);
                baseServer.Start();
                baseServer.Listen(5);

            }

            #region catch

            catch (Exception e)
            {
                Log.Error($"{e.Message}\r\n{e.InnerException?.Message}");
            }

            #endregion

            finally
            {
                if(baseServer != null)
                {
                    baseServer.Stop();
                    Task.WaitAll(baseServer.taskList.ToArray()); 
                }
            }
            Log.Debug("Server stopped...");
            Console.ReadLine();
        }

        private static void StartLogger()
        {
            string logfile = Directory.GetCurrentDirectory() + "/log.txt";

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()
                .WriteTo.Console(
                    LogEventLevel.Verbose,
                    "{NewLine}{Timestamp:HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}")
                .WriteTo.File(logfile, LogEventLevel.Verbose,
                    "{NewLine}{Timestamp:HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}", rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}