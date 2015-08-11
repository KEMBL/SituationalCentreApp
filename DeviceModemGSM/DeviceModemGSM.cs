using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace DeviceModemGSMLib
{
    /// <summary>
    /// This class describes how to manage GSM device with a serial COM interface
    /// For now used Cinterion MC52i REVISION 01.201
    /// </summary>
    public class DeviceModemGSM : IEventsDevice, IDisposable
    {

        #region StringConstants
        /// <summary>
        /// Apropriate GSM modem device name
        /// </summary>
        private const string DEVICE_NAME = "Cinterion";
        /// <summary>
        /// Command for request device name
        /// </summary>
        private const string COMMAND_GET_DEVICE_NAME = "AT+CGMI";
        /// <summary>
        /// AT Answer string OK
        /// </summary>        
        private const string Answer_OK = "\r\nOK\r\n";
        /// <summary>
        /// AT Answer string Greater at begin
        /// </summary>        
        private const string Answer_Greater = "\r\n> ";
        /// <summary>
        /// Answer string ERROR
        /// </summary>        
        private const string Answer_ERROR = "\r\nERROR\r\n";
        #endregion

        /// <summary>
        /// Some data for logging appeared
        /// </summary>
        public static Action<string> OnLogDataArrivedAction;
        /// <summary>
        /// Default COM port name where device is attached
        /// </summary>
        //private string Comm_Port_Name = "COM1";
        /// <summary>
        /// Default Baud Rate for COM device
        /// </summary>
        private int Baud_Rate = 9600;
        /// <summary>
        /// How long to wait for data in income buffer.
        /// </summary>
        private int ReadTimeout = 500;
        /// <summary>
        /// Find COM ports allowed to connect
        /// </summary>
        static public List<string> FindExistsComPorts
        {
            get { return SerialPort.GetPortNames().ToList(); }
        }
        /// <summary>
        /// reference to serial device
        /// </summary>
        private SerialPort SerialPort;
        /// <summary>
        /// Device operable or something else
        /// </summary>
        private DeviceState _status;
        /// <summary>
        /// Getter for device status
        /// </summary>
        public DeviceState DeviceStatus
        {
            get { return _status; }
        }
        /// <summary>
        /// Class initialization
        /// </summary>
        public DeviceModemGSM(bool autoConnect = true)
        {
            SetStatus(DeviceState.Disconnected);
            if (autoConnect)
                SearchDevice();
        }
        /// <summary>
        /// Status of this device is changed
        /// </summary>
        public EventHandler<EventArgs> DeviceStateChange { get; set; }        
        /// <summary>
        /// This event will signalize when need to read data from device
        /// </summary>
        private AutoResetEvent _readDeviceBuffer;
        /// <summary>
        /// This thread for probing comports, it will stay active for receiving messages when appropriate COM port is found
        /// </summary>
        private Thread _probeTread;
        /// <summary>
        /// Search for appropriate device among computer COM devices
        /// </summary>
        public void SearchDevice()
        {
            //connecting device
            if (_status != DeviceState.Disconnected )
                Disconnect();

            SetStatus(DeviceState.Connecting);

            //search COM port with connected Cinterion GSM modem
            foreach (var comPort in FindExistsComPorts)
            {                
                lock (this) //do not allow to request COM ports simultaneously
                {
                    if (_status == DeviceState.Connecting) //not connected yet
                    {
                        _probeTread = new Thread(() => TryPort(comPort));
                        _probeTread.Start(); //start probing a port in background thread
                        
                        // wait a little while a try ends
                        DateTime waitUntl = DateTime.Now + new TimeSpan(0, 0, 0, 0, 500);
                        do
                        {
                            Thread.Sleep(new TimeSpan(0, 0, 0, 0, 50)); //as tests show, even when port is right it requires up to 150-200 msec for connecting to it and check device name on it
                        }
                        while (SerialPort == null && // appropriate serial port is not yet found
                            waitUntl > DateTime.Now && // port check timeout is not yet reached
                            _status != DeviceState.Disconnected // something outer reasons did not set process status in to disconnect state
                            );
                        
                        // check what we found
                        if (SerialPort == null) //not found anything appropriate
                        {
                            LogData("SearchDevice(): GSM Device was not connected in time on port " + comPort);
                            Disconnect(); //let threads exit by disconnect status                             
                            _probeTread.Join(); // wait while probe thread ends
                            SetStatus(DeviceState.Connecting); //we will try once more port
                        }
                        else  //found            
                        {
                            if (_probeTread.IsAlive)
                                _probeTread.Join(); // wait while probe thread ends, actually it did it already
                            SetStatus(DeviceState.Connected);
                            break; //the search ends
                        }
                        //else try the next port
                    }
                }
            }

            if (_status == DeviceState.Connecting) //status still do not change, assume we cant connect to appropriate COM port device
                SetStatus(DeviceState.Disconnected);
        }
        /// <summary>
        /// IDispose realization
        /// </summary>
        public void Dispose()
        {
#if DEBUG
            Console.WriteLine("-------Dispose--------");
#endif
            if (_status != DeviceState.Disconnected)
                Disconnect();

            //unlink event listeners
            DeviceStateChange = null; //DO NOT MOVE IT TO DISCONNECT
        }
        /// <summary>
        /// Detach from modem device if it exists
        /// </summary>
        public void Disconnect()
        {
            //searchPort
            SetStatus(DeviceState.Disconnected);

            if (_readDeviceBuffer != null)
                _readDeviceBuffer.Dispose();

            if (_probeTread != null && _probeTread.IsAlive)
                _probeTread.Join();

            if (SerialPort != null)
            {
                SerialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(ErrorReceived);
                SerialPort.DataReceived -= new SerialDataReceivedEventHandler(OnDataRecieve);                
                SerialPort.Close(); //close COM connection                
                SerialPort = null;
            }
        }
        /// <summary>
        /// Set current device connection status
        /// </summary>
        /// <param name="status"></param>
        private void SetStatus(DeviceState status)
        {
            _status = status;
            if (DeviceStateChange != null)
                DeviceStateChange(_status, null);
        }
        /// <summary>
        /// Try open port and test 
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        private SerialPort TryPort(string portName)
        {
            // make new signaler event
            _readDeviceBuffer = new AutoResetEvent(false);
            try
            {
                // try to open serial port 
                using (SerialPort serial = new SerialPort(portName, Baud_Rate, Parity.None, 8, StopBits.One)
                {
                    Handshake = Handshake.None,
                    ReadTimeout = 200,
                    WriteTimeout = 500,
                    Encoding = Encoding.GetEncoding("iso-8859-1")
                })
                {
                    //add callbacks
                    serial.ErrorReceived += new SerialErrorReceivedEventHandler(ErrorReceived);
                    serial.DataReceived += new SerialDataReceivedEventHandler(OnDataRecieve);

                    //connect                
                    serial.Open();
                    serial.DtrEnable = true;
                    serial.RtsEnable = true;

                    if (serial.IsOpen)
                    {
                        // try connect and request device name
                        string device = SendComand(serial, COMMAND_GET_DEVICE_NAME);

                        if (_status == DeviceState.Disconnected)
                            return null; //we was disconnected from device

                        if (device.IndexOf(DEVICE_NAME, StringComparison.Ordinal) > -1)
                        {
                            LogData(string.Format("TryPort(): Appropriate device found on port {0}", serial.PortName));
                            SerialPort = serial;
                            return serial;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogData("TryPort(): Exception1: " + ex.Message);
            }
            return null;
        }
        
        #region Transmitting
        /// <summary>
        /// Send command to GSM modem
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string SendCommand(string command)
        {
            if (SerialPort == null)
            {
                LogData(string.Format("SendComand(1): Error SerialPort not initialised, command: {0}", command));
                return null;
            }

            if (!SerialPort.IsOpen)
                SerialPort.Open();
            
            if (SerialPort.IsOpen)
                return SendComand(SerialPort, command);

            LogData(string.Format("SendComand(2): Error SerialPort not open, command: {0}", command));
            return null;
        }
        /// <summary>
        /// Send command to modem and read result
        /// </summary>
        /// <param name="serial"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        private string SendComand(SerialPort serial, string command)
        {
            string incomeMessage = string.Empty;
            try
            {
                //clean buffers
                serial.DiscardOutBuffer();
                serial.DiscardInBuffer();
                _readDeviceBuffer.Reset(); //reset state and use that event to wait for new message from device                
                
                //exec command
                serial.Write(command + "\r");
                
                //read command result
                incomeMessage = ListenForIncomeMessages(serial);

                if (_status == DeviceState.Disconnected)
                    return string.Empty; //we disconnected from device

                if ( (incomeMessage.Length == 0) ||
                     (!incomeMessage.EndsWith(Answer_Greater) && !incomeMessage.EndsWith(Answer_OK))
                   )
                    LogData("Message receiving error, probable income data is incomplete: '" + incomeMessage + "'");

                return incomeMessage;
            }
            catch (Exception ex)
            {
                LogData("Exception2: " + ex.Data);                
            }

            return incomeMessage;
        }
        #endregion

        #region Receiving
        /// <summary>
        /// Listen data on COM port
        /// </summary>
        /// <param name="serial"></param>
        private string ListenForIncomeMessages(SerialPort serial)
        {
            string message = string.Empty;
            try
            {
                do
                {
                    if (_status == DeviceState.Disconnected)
                    {
                        return string.Empty; //we disconnected from device while reading buffer
                    }

                    if (_readDeviceBuffer.WaitOne(ReadTimeout, false))
                    {
                        message += serial.ReadExisting(); //increase received data
                    }
                    else
                    {
                        if (message.Length > 0)
                            LogData(string.Format("ListenForIncomeMessages(): ReadTimeout on port {0}. Some data in buffer but it seems not all data was received: '{1}'", serial.PortName, message));
                        else
                            LogData(string.Format("ListenForIncomeMessages(): ReadTimeout on port {0}. income buffer still empty, try to increase ReadTimeout.", serial.PortName));
                    }
                }
                while (!message.EndsWith(Answer_OK) && !message.EndsWith(Answer_Greater) && !message.EndsWith(Answer_ERROR));
            }
            catch (Exception ex)
            {
                LogData("Exception3, ListenForIncomeMessages(): " + ex.Message);                
            }
            return message;
        }
        /// <summary>
        /// Callback for receiving information from serial port
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDataRecieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars) // we got chars from device
                {
                    //since we read income data from another Thread we need sent signal that some data ready to read from device buffer!
                    _readDeviceBuffer.Set(); //signalize that it needs to read data from device buffer
                }
            }
            catch (Exception ex)
            {
                LogData("Receive(): Exception. Error " + ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// COM Error received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            LogData(string.Format("ErrorReceived():  {0}, {1} ", sender.GetType().Name, e));
        }
        #endregion

        /// <summary>
        /// Some data needs to be logged
        /// </summary>
        /// <param name="newdata"></param>
        private void LogData(string newdata)
        {
#if DEBUG
            Console.WriteLine("> {0}", newdata);
#endif
            if (OnLogDataArrivedAction != null)
            {
                OnLogDataArrivedAction(newdata);
            }
        }
    }
}
