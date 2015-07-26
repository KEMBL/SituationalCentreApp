using System;
using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Fakes;
using Moq;

namespace SituationalCentreApp_UnityTests
{
    [TestClass]
    public class InitialisationTests
    {
        [TestMethod]
        public void SituationalCentreActivation()
        {
            Mock<IEventsProvider> eventsProvider = new Mock<IEventsProvider>();
            Mock<IDataStorage> dataStorage = new Mock<IDataStorage>();
            
            SituationalCentre sCentre = new SituationalCentre(eventsProvider.Object, dataStorage.Object);

            Assert.IsNotNull(sCentre);
        }
    }
}
