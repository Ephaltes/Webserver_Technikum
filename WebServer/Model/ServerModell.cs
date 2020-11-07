using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using WebServer.API;
using WebServer.Interface;

namespace WebServer.Model
{
    public class ServerModell : IServer , IDisposable
    {
        private SemaphoreSlim _semaphore;
        private readonly TcpListener _listener;
        private readonly List<Task> _taskList;
        public List<Task> taskList => _taskList;
        public bool IsRunning { get; set; } = false;


        public ServerModell(IPAddress ipAddress ,int port)
        {
            _listener = new TcpListener(ipAddress,port);
            _taskList = new List<Task>();
        }
        
        public void Start()
        {
           _listener.Start();
           IsRunning = true;
        }

        public void Stop()
        {
            _listener.Stop();
            IsRunning = false;
        }

        public void Listen(int maxClient)
        {
            if(!IsRunning)
                Start();
            
            _semaphore = new SemaphoreSlim(maxClient);
            while (true)
            {
                _semaphore.Wait();
                _taskList.Add(Task.Run(HandleClient));
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public virtual void HandleClient()
        {
            Log.Debug("Waiting for a connection... ");
            try
            {
                Model.TcpClient client = new Model.TcpClient(_listener.AcceptTcpClient());
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
            _semaphore.Release();
        }

        public void Dispose()
        {
            _semaphore?.Dispose();
            _listener.Stop();
        }
    }
}