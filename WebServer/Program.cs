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
        static SemaphoreSlim semaphore = new SemaphoreSlim(5);

        static void Main(string[] args)
        {
            StartLogger();

            TcpListener server = null;
            List<Task> taskList = new List<Task>();


            try
            {
                server = new TcpListener(IPAddress.Parse(AppSettings.Settings.Host), AppSettings.Settings.Port);
                server.Start();

                while (true)
                {
                    semaphore.Wait();
                    taskList.Add(Task.Run(() => HandleClient(server)));
                }
            }

            #region catch

            catch (Exception e)
            {
                Log.Error($"{e.Message}\r\n{e.InnerException?.Message}");
            }

            #endregion

            finally
            {
                server?.Stop();
                Task.WaitAll(taskList.ToArray());
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
        
        public static void HandleClient(TcpListener server)
        {
            Log.Debug("Waiting for a connection... ");
            try
            {
                Model.TcpClient client = new Model.TcpClient(server.AcceptTcpClient());
                Log.Debug($"Client {client.RemoteEndPoint} connected");

                ApiController controller = new ApiController(client);
                controller.Respond(controller.CreateResponse());

                Log.Debug($"Client {client.RemoteEndPoint} disconnected\r\n");
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            semaphore.Release();
        }
    }
}
