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
    class CommandSender
    {
        private string ip = Properties.Settings.Default.FlightServerIP;
        private int port = Properties.Settings.Default.FlightCommandPort;
        private IPEndPoint ep;
        private TcpClient client = new TcpClient();
        private Socket s = null;
        private Dictionary<string, string> pathDict = new Dictionary<string, string>();
        public volatile bool isSenderConnected = false;
        public volatile bool abort = false; // for immidiate cancel


        // private constructor
        private CommandSender()
        {
            InitDict();
            ep = new IPEndPoint(IPAddress.Parse(ip), port);
            s = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        }
        public string Ip { get; set; }
        public int Port { get; set; }

        #region Singleton
        private static CommandSender m_Instance = null;
        public static CommandSender Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new CommandSender();
                }
                return m_Instance;
            }
        }
        #endregion
        // connect function
        public void connectToServer()
        {
            Thread thread = new Thread(() =>
            {
                ep = new IPEndPoint(IPAddress.Parse(ip), port);
                s = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // try to connect
                while (!s.Connected && !abort)
                {
                    try
                    {
                        s.Connect(ep);
                    }
                    catch
                    {
                        continue;
                    }
                    
                }
                //Console.WriteLine("CONNECTEDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD");
                isSenderConnected = true;
                abort = false;
                //client.Connect(ep);
                client.Client = s;
                Console.WriteLine("You are connected");
            });
            thread.Start();
        }
        public void sendDataAsIs(string data)
        {
            // Send data to server
            NetworkStream stream = client.GetStream();
            BinaryWriter writer = new BinaryWriter(stream);
            string[] dataArray = ParseLine(data);
            AddEnter(dataArray);
            foreach (string dataItem in dataArray)
            {
                Console.WriteLine(dataItem);
                Byte[] bytesSent = Encoding.ASCII.GetBytes(dataItem);
                s.Send(bytesSent, bytesSent.Length, 0);
                // sleep for 2 seconds between each command
                System.Threading.Thread.Sleep(2000);
            }
        }

        public void sendDataWithSet(double dataValue, string planePart)
        {
            // Send data to server
            string toSend = EditSetStr(dataValue, planePart);
            //Console.WriteLine(toSend);
            Byte[] bytesSent = Encoding.ASCII.GetBytes(toSend);
            s.Send(bytesSent, bytesSent.Length, 0);
        }

        // close socket function
        public void close()
        {
            client.Close();
            s.Close();
            isSenderConnected = false;
            abort = true;
        }
        // split input by '\n'
        public string[] ParseLine(string line)
        {
            //line += "\r\n";
            //string[] stringSeparators = new string[] { "\r\n" };
            string[] stringSeparators = new string[] {Environment.NewLine};
            string[] tokens = line.Split(stringSeparators, StringSplitOptions.None);
            //string[] tokens = line.Split('\n');
            return tokens;
        }
        // add "\r\n" function
        public void AddEnter(string[] array)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                //if (array[i].Length <= 2) 
                //{
                //    continue;
                //}
                //if (array[i][array[i].Length] == '\r')
                //{
                //    array[i] += '\n';
                //}
                //else
                //{
                //    array[i] += "\r\n";
                //}
                array[i] += "\r\n";
            }
        }
        // a function to prepare the data to send according to the simulator syntax
        public string EditSetStr(double dataValue, string planePart)
        {
            string res = "set ";
            string path = pathDict[planePart];
            res = res + path + " " + dataValue + "\r\n";
            return res;
        }

        // initialize dictionary function
        public void InitDict()
        {
            pathDict.Add("rudder", "controls/flight/rudder");
            pathDict.Add("throttle", "controls/engines/current-engine/throttle");
            pathDict.Add("aileron", "controls/flight/aileron");
            pathDict.Add("elevator", "controls/flight/elevator");
        }
    }
}
