using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.Views;
using FlightSimulator.ViewModels;
using FlightSimulator.Model;
using System.Windows.Controls;
using FlightSimulator.ViewModels.Windows;
using System.Threading;

namespace FlightSimulator.ViewModels
{
    class AutoPilotVM : BaseNotify
    {
        private ICommand okCommand; // for ok button
        private ICommand clearCommand; // for clear button
        private string textBoxStr = "";
        private string backgroundStr;
        public CommandSender commandSender = CommandSender.Instance;
        //private bool makeWhite = false;

        public ICommand OkCommand
        {
            get
            {
                // return okCommand that is not null
                return okCommand ?? (okCommand = new CommandHandler (() => OkClick()));
            }
        }
        public void OkClick()
        {
            if (textBoxStr == "") { return; } // if ok was pressed again on an empty text, do nothing
            Thread thread = new Thread(() =>
           {
               // send the data to the simulator as is
               commandSender.sendDataAsIs(textBoxStr);
           });
            thread.Start();
            thread.Join();
            //            Console.WriteLine("sent****************************************");
            //            makeWhite = true;

            // clear the text box
            TextBoxStr = "";
            NotifyPropertyChanged("TextBoxStr");
            NotifyPropertyChanged("BackgroundStr");
            
        }

        public ICommand ClearCommand
        {
            get
            {
                // return a clear command that is not null
                return clearCommand ?? (clearCommand = new CommandHandler(() => ClearClick()));
            }
        }

        // text box property
        public string TextBoxStr
        {
            get { return textBoxStr; }
            set
            {
                //if ((backgroundStr == "White") && (value.Length > 0))
                //{
                //    string temp = "";
                //    temp += value[0];
                //    textBoxStr = "";
                //    NotifyPropertyChanged("TextBoxStr");
                //    textBoxStr = temp;
                //}
                //else
                //{
                //    textBoxStr = value;
                //}
                textBoxStr = value;
                NotifyPropertyChanged("TextBoxStr");
                NotifyPropertyChanged("BackgroundStr");
            }
        }
        public void ClearClick()
        {
            // empty the text and so the background will be white
            TextBoxStr = "";
        }

        public string BackgroundStr
        {
            get
            {
                if (textBoxStr == "")
                {
                    backgroundStr = "White";
//                    makeWhite = false;
                }
                else
                {
                    backgroundStr = "Pink";
                }
                return backgroundStr;
            }
        }
    }
}
