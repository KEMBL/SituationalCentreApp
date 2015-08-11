using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceModemGSMLib
{
    /// <summary>
    /// This interface allows to use different devices as event sources
    /// </summary>
    public interface IEventsDevice
    {
        /// <summary>
        /// Getter for device status, Operable == true
        /// </summary>
        DeviceState DeviceStatus { get; }

        /// <summary>
        /// Call back to read device state changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void DeviceStateChange(EventHandler<EventArgs> subscribeHandler);
        EventHandler<EventArgs> DeviceStateChange { set; get; }
        /// <summary>
        /// Start to search appropriate device among computer devices
        /// </summary>
        void SearchDevice();
        /// <summary>
        /// Detach from modem device if it exists
        /// </summary>
        void Disconnect();
        /// <summary>
        /// Send command to device
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        string SendCommand(string command);
    }
}
