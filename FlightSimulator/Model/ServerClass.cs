using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator.Model
{
    class ServerClass
    {
        private int port = Properties.Settings.Default.FlightInfoPort;
        private string ip = Properties.Settings.Default.FlightServerIP;
        private TcpListener listener;
        private IClientHandler ch = new ClientHandler();
        private IPEndPoint ep;
        private Task task;
        private Thread t;
        public volatile bool isServerClassConnected = false;

        public int Port { set; get; }
        public string Ip { set; get; }
        public IClientHandler ICLientHandler { set; get; }
        public bool IsConnected { set; get; }

        #region Singleton
        private static ServerClass m_Instance = null;
        public static ServerClass Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ServerClass();
                }
                return m_Instance;
            }
        }
        #endregion
        public void Start()
        {
            // start lintenning
            ep = new IPEndPoint(IPAddress.Parse(ip), port);
            listener = new TcpListener(ep);
            listener.Start();
            Console.WriteLine("Waiting for connections...");

            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = listener.AcceptTcpClient();
                        Console.WriteLine("Got new connection");
                        isServerClassConnected = true;
                        t = new Thread(() => {
                            // send the clinent to the clientHandler 
                            ch.HandleClient(client);
                        });
                        t.Start();
                    }
                    catch (SocketException)
                    {
                        break;
                    }
                }
                Console.WriteLine("Server stopped");
            });
            thread.Start();
           
            
        }

        // a function to stop the server
        public void Stop()
            {
                listener.Stop();
                t.Abort();
                isServerClassConnected = false;
                
            }
    }
}
