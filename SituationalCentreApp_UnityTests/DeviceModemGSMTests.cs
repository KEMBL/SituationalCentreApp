using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeviceModemGSMLib;
using System.Threading;

namespace SituationalCentreApp_UnityTests
{
    /// <summary>
    /// Tests for GSM Messaging device
    /// </summary>
    [TestClass]
    public class DeviceModemGSMTests
    {
        [TestMethod]
        public void SearchDeviceTest()
        {
            //this is asynchronous task so make it in with threads
            IEventsDevice eventsDevice = new DeviceModemGSM();
            
            DeviceState deviceState = eventsDevice.DeviceStatus; //read device status, probable it was found already in constructor
            if (deviceState != DeviceState.Conected) //was not found
            {
                //when new status arrives we will cancel Monitor.Wait
                eventsDevice.DeviceStateChange += (s, e) => { Monitor.Pulse(this); };

                lock (this) 
                {
                    eventsDevice.SearchDevice(); //start to search device
                    deviceState = eventsDevice.DeviceStatus; //read device status, we might found device already
                    if (deviceState != DeviceState.Conected) //not found
                    {
                        //wait for results more time
                        if (!Monitor.Wait(this, new TimeSpan(0, 0, 1))) //wait for 3 sec
                            Assert.Fail("GSM DeviceFound did not find in time!");

                        deviceState = eventsDevice.DeviceStatus; //read status
                    }
                }

                eventsDevice.DeviceStateChange = null;
            }

            //check status
            Assert.AreEqual(deviceState, DeviceState.Conected);
        }
    }
}
