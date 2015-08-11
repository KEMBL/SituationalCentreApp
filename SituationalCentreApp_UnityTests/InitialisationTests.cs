using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Fakes;
using Moq;

using BusinessLogic;
using DataStorageProviderLib;
using DeviceModemGSMLib;
using EventsProviderLib;
using XMLDataStorageLib;

namespace SituationalCentreApp_UnityTests
{
    [TestClass]
    public class InitialisationTests
    {
        /// <summary>
        /// Test of main application object creation
        /// </summary>
        [TestMethod]
        public void SituationalCentre_NewSituationalCentre()
        {
            // create temporary interfaces for unexists yet classes
            Mock<IEventsProvider> eventsProvider = new Mock<IEventsProvider>();
            Mock<IDataStorageProvider> dataStorage = new Mock<IDataStorageProvider>();

            // Run preparation steps to initialization application            
            SituationalCentre sCentre = new SituationalCentre(eventsProvider.Object, dataStorage.Object);

            //check results            
            Assert.IsTrue(sCentre.Status);
        }
        /// <summary>
        /// Test class which allows to store and read persistent information for this application
        /// </summary>
        [TestMethod]
        public void XMLDataStorage_Initialization()
        {
            IDataStorage xmlStorage = new XMLDataStorage();

            //storage initialized well
            Assert.IsTrue(xmlStorage.StoreLoaded);
        }
        /// <summary>
        /// Test class which provides data storage behavior
        /// </summary>
        [TestMethod]
        public void DataStorageProvider_Initialization()
        {
            IDataStorage xmlStorage = new XMLDataStorage();
            IDataStorageProvider dataStoreProvider = new DataStorageProvider(xmlStorage);

            Assert.IsNotNull(dataStoreProvider);
        }
        /// <summary>
        /// Test class which provides events for processing in BusinessLogic
        /// </summary>
        [TestMethod]
        public void EventsProvider_Initialisation()
        {
            Mock<IEventsDevice> eventsDevice = new Mock<IEventsDevice>();

            IEventsProvider eventsProvider = new EventsProvider(eventsDevice.Object);
            
            Assert.IsNotNull(eventsProvider);
        }
        /// <summary>
        /// Test of main application class initialization
        /// </summary>
        [TestMethod]
        public void SituationalCentre_Initialization()
        {
            // Run preparation steps to initialization application            
            SituationalCentre sCentre = SituationalCentre.Initialization();

            //check results
            Assert.IsNotNull(sCentre);
        }
    }
}
