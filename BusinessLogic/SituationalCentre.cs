using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorageProviderLib;
using EventsProviderLib;

namespace BusinessLogic
{
    public class SituationalCentre
    {
        /// <summary>
        /// Application engine started well
        /// </summary>
        public bool Status;
        /// <summary>
        /// Interface for events sources collected from outer world trough some device
        /// </summary>
        private readonly IEventsProvider _eventsProvider;
        /// <summary>
        /// Interface storage provider which helps to store persistent data
        /// </summary>
        private readonly IDataStorageProvider _dataStorage;
        /// <summary>
        /// Constructor for class which performs all main application activities - Receive, show and send events
        /// </summary>
        /// <param name="eventsProvider"></param>
        /// <param name="dataStorage"></param>
        public SituationalCentre(IEventsProvider eventsProvider, IDataStorageProvider dataStorage)
        {
            // TODO: Complete member initialization
            _eventsProvider = eventsProvider;
            _dataStorage = dataStorage;

            Status = true;
        }
    }
}
