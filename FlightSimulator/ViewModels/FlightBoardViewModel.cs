using FlightSimulator.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightSimulator.ViewModels
{
    public class FlightBoardViewModel : BaseNotify
    {
        private double lon;
        private double lat;

        // make this class a singelton
        #region Singleton
        private static FlightBoardViewModel m_Instance = null;
        public static FlightBoardViewModel Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new FlightBoardViewModel();
                }
                return m_Instance;
            }
        }
        #endregion

        // lon property
        public double Lon
        {
            get { return lon; }
            set
            {
                lon = value;
                NotifyPropertyChanged("Lon");
            }
        }

        // lat property
        public double Lat
        {
            get { return lat; }
            set
            {
                lat = value;
                NotifyPropertyChanged("Lat");
            }
        }
    }
}
