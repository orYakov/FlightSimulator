using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightSimulator.Views;
using System.Threading;

namespace FlightSimulator.ViewModels
{
    class ManualVM : BaseNotify
    {
        private double rudder;
        private double throttle;
        private double aileron;
        private double elevator;
        public CommandSender commandSender = CommandSender.Instance;
        public double Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                if (!commandSender.isSenderConnected)
                {
                    return;
                }

                Thread thread = new Thread(() =>
                {
                    commandSender.sendDataWithSet(Rudder, "rudder");
                });
                thread.Start();
                //thread.Join();
                NotifyPropertyChanged("Rudder");
                //Console.WriteLine("sent form joystick****************************************");
            }
        }

        public double Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                if (!commandSender.isSenderConnected)
                {
                    return;
                }

                Thread thread = new Thread(() =>
                {
                    commandSender.sendDataWithSet(Throttle, "throttle");
                });
                thread.Start();
                //thread.Join();
                NotifyPropertyChanged("Throttle");
                //Console.WriteLine("sent form joystick****************************************");
            }
        }

        public double Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                if (!commandSender.isSenderConnected)
                {
                    return;
                }

                Thread thread = new Thread(() =>
                {
                    commandSender.sendDataWithSet(Aileron, "aileron");
                });
                thread.Start();
                //thread.Join();
                NotifyPropertyChanged("Aileron");
                //Console.WriteLine("sent form joystick****************************************");
            }
        }

        public double Elevator
        {
            get { return elevator; }
            set
            {
                elevator = value;
                if (!commandSender.isSenderConnected)
                {
                    return;
                }

                Thread thread = new Thread(() =>
                {
                    commandSender.sendDataWithSet(Elevator, "elevator");
                });
                thread.Start();
                //thread.Join();
                NotifyPropertyChanged("Elevator");
                //Console.WriteLine("sent form joystick****************************************");
            }
        }
    }
}
