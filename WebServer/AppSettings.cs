using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace WebServer
{
    class AppSettings
    {

        private static AppSettings _settings = null;

        private static readonly object lockobject = new object();

        private readonly int _port;
        private readonly string _host;
        private readonly string _path;

        public int Port => Settings._port;

        public string Host => Settings._host;
        public string Path => Settings._path;

        private const int DEFAULTPORT = 50000;
        private const string DEFAULTHOST = "127.0.0.1";

        /// <summary>
        /// Singleton Instance of AppSettings
        /// </summary>
        public static AppSettings Settings
        {
            //Prüft ob es null ist wenn null dann leg für _settings eine neue AppSettings an
            get
            {
                lock (lockobject)
                {
                    return _settings ??= new AppSettings();
                }
            }
        }

        private AppSettings()
        {

            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            var root = configurationBuilder.Build();

            var server = root.GetSection("Server");
            try
            {
                _port = Convert.ToInt32(server.GetSection("Port").Value);
                _port = _port == 0 ? DEFAULTPORT : _port;
            }
            catch (Exception e)
            {
                _port = DEFAULTPORT;
            }

            _host = server.GetSection("Host").Value;
            if (String.IsNullOrWhiteSpace(_host))
                _host = DEFAULTHOST;

            _path = server.GetSection("Path").Value;
            if (String.IsNullOrWhiteSpace(_path))
                _path = Directory.GetCurrentDirectory();


        }
    }
}
