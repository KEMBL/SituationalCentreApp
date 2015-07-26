using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeviceModemGSMLib;

namespace EventsProviderLib
{
    /// <summary>
    /// This object is glue between main application and some event source device
    /// </summary>
    public class EventsProvider : IEventsProvider
    {
        /// <summary>
        /// Device - the source of outer events and the target for sending the events
        /// </summary>
        IEventsDevice _eventsDevice;
        /// <summary>
        /// Initialization of event device
        /// </summary>
        /// <param name="eventsDevice"></param>
        public EventsProvider(IEventsDevice eventsDevice)
        {
            // TODO: Complete member initialization
            this._eventsDevice = eventsDevice;
        }

    }
}
