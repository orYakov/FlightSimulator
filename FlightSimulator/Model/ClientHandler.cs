using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using FlightSimulator.Views;


namespace FlightSimulator.Model
{
    class ClientHandler : IClientHandler
    {
        // get the values from the simulator and update Lon and Lat properties
        public void HandleClient(TcpClient client)
        {
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);
                while (true)
                {
                    {
                        string commandLine = reader.ReadLine();
                        string[] result = ParseLine(commandLine);
                        FlightBoardViewModel.Instance.Lon = float.Parse(result[0]);
                        FlightBoardViewModel.Instance.Lat = float.Parse(result[1]);
                        //Console.WriteLine("[{0}]", string.Join(" ", result));                        
                    }
                }
        }
        // split the input according to ','
        public string[] ParseLine(string commandLine)
        {
            string[] tokens = commandLine.Split(',');
            return tokens;
        }
    }

}
