using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Views;
using FlightSimulator.Model;
using System.Windows.Input;

namespace FlightSimulator.ViewModels
{
    class connectAndSettingsVM : BaseNotify
    {
        private ICommand settingsCommand;
        private ICommand connectCommand;
        private ICommand disConnectCommand;
        private ServerClass server;
        private CommandSender sender;
        private bool isConnected = false;
        
        
        public ICommand SettingsCommand
        {
            get
            {
                return settingsCommand ?? (settingsCommand = new CommandHandler(() => OnClick()));
            }
        } 
        public void OnClick()
        {
            // create a new settings window
            Settings stt = new Settings();
            stt.ShowDialog();
        }

        public ICommand ConnectCommand
        {
            get
            {
                return connectCommand ?? (connectCommand = new CommandHandler(() => ConnectClick()));
            }
        }

        public void ConnectClick()
        {
            if (!isConnected)
            {
                //Console.WriteLine("Creating server*****************************************");
                // create a server
                server = ServerClass.Instance;
                server.Start();
                sender = CommandSender.Instance;
                // connect as a client
                sender.connectToServer();
                isConnected = true;
            }
            //server.Stop();
        }

        public ICommand DisConnectCommand
        {
            get
            {
                return disConnectCommand ?? (disConnectCommand = new CommandHandler(() => DisConnectClick()));
            }
        }

        public void DisConnectClick()
        {
            if (isConnected)
            {
                server.Stop();
                //Console.WriteLine("bye bye server*****************************************");
                sender.close();
                //Console.WriteLine("bye bye client*****************************************");
                isConnected = false;
            }
        }
    }
}
