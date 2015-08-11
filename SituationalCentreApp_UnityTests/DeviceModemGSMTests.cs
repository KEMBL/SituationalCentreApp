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
        /// <summary>
        /// Test class for serial GSM modem device which is a source of all events
        /// </summary>
        [TestMethod]
        public void GSMModem_Initialisation()
        {
            IEventsDevice eventsDevice = new DeviceModemGSM();
            // check device status
            //Assert.IsTrue(eventsDevice.DeviceStatus);
            Assert.AreEqual(eventsDevice.DeviceStatus, DeviceState.Connected);
            eventsDevice.Disconnect();
            Assert.AreEqual(eventsDevice.DeviceStatus, DeviceState.Disconnected);
        }
        [TestMethod]
        public void SearchDeviceTest()
        {
            //this is asynchronous task so make it in with threads
            IEventsDevice eventsDevice = new DeviceModemGSM(false);
            
            DeviceState deviceState = eventsDevice.DeviceStatus; //read device status, probable it was found already in constructor
    
            //when new status arrives we will cancel Monitor.Wait
            eventsDevice.DeviceStateChange += (s, e) =>
            {
                if (((DeviceState)s) == DeviceState.Connected)
                {
                    lock (this) // need for Pulse
                    {
                        Monitor.Pulse(this); //release thread waiting  - проблема - Pulse не активирует Wait ниже
                    }
                }
            };

            Thread searchPort = null;
            lock (this) // lock need for Monitor.Wait to avoid SynchronizationLockException
            {
                searchPort = new Thread(() => eventsDevice.SearchDevice());
                searchPort.Start(); //start to search device                    

                deviceState = eventsDevice.DeviceStatus; //read device status, we might found device already
                if (deviceState != DeviceState.Connected) //not found yet
                {
                    //wait for results more time
                    if (!Monitor.Wait(this, new TimeSpan(0, 0, 1)))
                        //wait for a sec - this depend on amount of COM devices one device ~ 1 second for test.
                    {
                        Assert.Fail("GSM Device was not found in time " + DateTime.Now);
                    }

                    deviceState = eventsDevice.DeviceStatus; //read status
                }
            }

            eventsDevice.DeviceStateChange = null; //try stop threads                                                    
            eventsDevice.Disconnect();
            //Thread.Sleep(200);
            if(searchPort.IsAlive)
                searchPort.Join();

            //check status
            Assert.AreEqual(deviceState, DeviceState.Connected);
        }
        /// <summary>
        /// Test class for serial GSM modem device which is a source of all events
        /// </summary>
        [TestMethod]
        public void GSMModem_SendCommand()
        {
            IEventsDevice eventsDevice = new DeviceModemGSM();
            // check device status
            string answer = string.Empty;
            Assert.AreEqual(eventsDevice.DeviceStatus, DeviceState.Connected);
            
            //test command 1
            answer = eventsDevice.SendCommand("AT");
            Assert.IsNotNull(answer);
            answer = answer.Replace(Environment.NewLine, "");
            Assert.AreEqual(answer, "OK");
            Console.WriteLine("1> '{0}'",  answer);

            //test command 2            
            answer = eventsDevice.SendCommand("AT+CPBR=1");
            Assert.IsNotNull(answer);
            answer = answer.Replace(Environment.NewLine, "");
            Assert.IsTrue(answer.IndexOf("+CPBR") > -1);
            Console.WriteLine("2> '{0}'", answer);

            //disconnect
            eventsDevice.Disconnect();
            Assert.AreEqual(eventsDevice.DeviceStatus, DeviceState.Disconnected);            
        }
    }
}
