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
        bool DeviceStatus { get; }
    }
}
