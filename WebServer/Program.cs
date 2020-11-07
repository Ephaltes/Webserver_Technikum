using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebServer
{
    class Program
    {
        static SemaphoreSlim semaphore = new SemaphoreSlim(5);

        static void Main(string[] args)
        {
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
                Console.WriteLine(e);
            }

            #endregion

            finally
            {
                server?.Stop();
                Task.WaitAll(taskList.ToArray());
            }
            Console.WriteLine("Server stopped...");
            Console.ReadLine();
        }

        public static void HandleClient(TcpListener server)
        {
            Console.WriteLine("Waiting for a connection... ");
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine($"Client {client.Client.RemoteEndPoint} connected");
            RequestContext requestContext = new RequestContext(client);

            Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected");
            client.Close();
            semaphore.Release();
        }
    }
}