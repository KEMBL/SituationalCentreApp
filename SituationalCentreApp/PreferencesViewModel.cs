using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SituationalCentreApp
{
    /// <summary>
    /// One Comm port item for Preferences window
    /// </summary>
    public class COMPortData
    {
        public int Id { get; set; }
        public string Value { get; set; }
#if DEBUG
        public override string ToString()
        {
            return "COM: " + Id + " -> " + Value;
        }
#endif
    }
    /// <summary>
    /// One BaudRateData item for Preferences window
    /// </summary>
    public class BaudRateData
    {
        public int Id { get; set; }
        public int Value { get; set; }
#if DEBUG
        public override string ToString()
        {
            return "BR: " + Id + " -> " + Value;
        }
#endif
    }

    /// <summary>
    /// View Model for Preferences window
    /// </summary>
    public class PreferencesViewModel
    {
        /// <summary>
        /// Value for COMPort caching
        /// </summary>
        private COMPortData _port;
        /// <summary>
        /// MVVM property for interface
        /// </summary>
        public COMPortData SelectedPort
        {
            get
            {
                if (_port == null) //not yet initialized
                {
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.COMPort)) //saved settings present
                    {
                        _port = _listCommPortsData.FirstOrDefault(var => var.Value == Properties.Settings.Default.COMPort);
#if DEBUG
                        if (_port == null)
                        {
                            Console.WriteLine("SelectedPort: Error ComPort is not found for item {0}", Properties.Settings.Default.COMPort);
                        }
#endif
                    }
                }
#if DEBUG
                Console.WriteLine("SelectedPort: GET Port: " + (_port != null ? _port.ToString() : "NULL"));
#endif
                return _port;
            }
            set  // New item has been selected
            {
                if (_port != value)
                {
                    _port = value;
                    Properties.Settings.Default.COMPort = _port.Value;
                    SaveOptions();//save to ROM
#if DEBUG
                    Console.WriteLine("Selected comport: " + _port);
#endif
                }
            }
        }
        /// <summary>
        /// Cache for Comm ports allowed to connect to
        /// </summary>
        private IList<COMPortData> _listCommPortsData;
        /// <summary>
        /// Getter for allowed Comm ports 
        /// </summary>
        public ObservableCollection<COMPortData> ComPorts
        {
            get
            {
                if (_listCommPortsData != null) //return cached values
                    return new ObservableCollection<COMPortData>(_listCommPortsData);

                // build new list
                _listCommPortsData = new List<COMPortData>();
                int cnt = 0;

                _listCommPortsData.Add(new COMPortData { Id = cnt++, Value = "COM1" });
                _listCommPortsData.Add(new COMPortData { Id = cnt++, Value = "COM24" });
                /* not yet implemented
                foreach (var port in COMManager.FindExistsComPorts)
                {
                    _listCommPortsData.Add(new COMPortData { Id = cnt++, Value = port });
                }
                 * */
                return new ObservableCollection<COMPortData>(_listCommPortsData);
            }
        }
        /// <summary>
        /// Cache value for current BoudRate settings
        /// </summary>
        private BaudRateData _boudRate;
        /// <summary>
        /// Getter for current baud rate settings
        /// </summary>
        public BaudRateData SelectedBoudRate
        {
            get
            {
                if (_boudRate == null) //not yet initialized
                {
                    if (Properties.Settings.Default.BaudRate > 0) //saved settings present
                    {
                        _boudRate = _baudData.FirstOrDefault(var => var.Value == Properties.Settings.Default.BaudRate);
#if DEBUG
                        if (_boudRate == null)
                        {
                            Console.WriteLine("SelectedBoudRate(1): Error baudDatahas is not found for item {0}", Properties.Settings.Default.BaudRate);
                        }
#endif
                    }
                }
                return _boudRate;
            }
            set // New item has been selected
            {
                if (_boudRate != value)
                {
                    _boudRate = value;
                    Properties.Settings.Default.BaudRate = _boudRate.Value;
                    SaveOptions(); //save to ROM
                }
            }
        }
        /// <summary>
        /// Cache for list of allowed baud rates
        /// </summary>
        private IList<BaudRateData> _baudData;
        /// <summary>
        /// Getter for the list of allowed baud rates
        /// </summary>
        public ObservableCollection<BaudRateData> BaudRates
        {
            get
            {
                if (_baudData != null) //have cached data?
                    return new ObservableCollection<BaudRateData>(_baudData);

                //build new settings array
                _baudData = new List<BaudRateData>(); //PERFORMANCE: cache it!!!
                int cnt = 0;
                foreach (var baud in Properties.Settings.Default.BaudRates)
                {
                    _baudData.Add(new BaudRateData { Id = cnt++, Value = int.Parse(baud) });
                }
#if DEBUG
                //Console.WriteLine("BaudRates: make {0} BoudRates: ", baudData.Count);
#endif
                return new ObservableCollection<BaudRateData>(_baudData);
            }
        }
        /// <summary>
        /// Save user settings to ROM
        /// </summary>
        void SaveOptions()
        {
            Properties.Settings.Default.Save();
        }
    }
}
