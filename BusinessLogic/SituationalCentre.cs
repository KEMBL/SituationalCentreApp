using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStorageProviderLib;
using DeviceModemGSMLib;
using EventsProviderLib;
using XMLDataStorageLib;

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
        /// <summary>
        /// Run preparation steps to initialization application
        /// </summary>
        /// <param name="eventsProvider"></param>
        /// <param name="dataStorageProvider"></param>
        /// <returns></returns>
        public static SituationalCentre Initialization()
        {
#if DEBUG
            Console.WriteLine("-------Initialization--------");
            Log.Debug("-------Initialization--------");
#endif

            //1. Init XML DATA Storage
            IDataStorage storage = new XMLDataStorage();
            if (!storage.StoreLoaded)
            {
#if DEBUG
                Console.WriteLine("Error Initialization(): Cannot init DataStorage.");
#endif
                return null;
            }

            return null;

            //2. create events device
            bool autoConnect = true;
            IEventsDevice eventsDevice = new DeviceModemGSM(autoConnect);

            //3. Init Events provider
            IEventsProvider eventsProvider = new EventsProvider(eventsDevice);
            IDataStorageProvider dataStorageProvider = null;

            SituationalCentre sCentre = new SituationalCentre(eventsProvider, dataStorageProvider);
            return sCentre;

            /// <summary>
            /// Start up all other components
            /// </summary>

            //1. Init XML DATA Storage
            //IDataStorage storage = new XMLDataStorage();

            //2. Init Data provider

            //3. Init Events provider

            //

            //4. SituationalCentre(IEventsProvider eventsProvider, IDataStorageProvider dataStorage)
            


        }
    }
}
