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
    /// <summary>
    /// ServerWrapper
    /// </summary>
    public class BaseServerModell : IServer, IDisposable
    {
        private SemaphoreSlim _semaphore;
        private readonly TcpListener _listener;
        private readonly List<Task> _taskList;
        public List<Task> taskList => _taskList;
        public bool IsRunning { get; set; } = false;

        /// <summary>
        /// Create the server on the ipAddress and Port
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public BaseServerModell(IPAddress ipAddress, int port)
        {
            _listener = new TcpListener(ipAddress, port);
            _taskList = new List<Task>();
        }

        /// <summary>
        /// Start Server
        /// </summary>
        public void Start()
        {
            _listener.Start();
            IsRunning = true;
        }

        /// <summary>
        /// Stop Server
        /// </summary>
        public void Stop()
        {
            _listener.Stop();
            IsRunning = false;
        }

        /// <summary>
        /// maxClient can connect at the same time to server
        /// </summary>
        /// <param name="maxClient"></param>
        public void Listen(int maxClient)
        {
            if (!IsRunning)
                Start();

            _semaphore = new SemaphoreSlim(maxClient);
            while (true)
            {
                _semaphore.Wait();
                _taskList.Add(Task.Run(HandleClient));
            }

            // ReSharper disable once FunctionNeverReturns
        }

        /// <summary>
        /// How each client is to be handled
        /// </summary>
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
                Log.Error(e.Message);
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