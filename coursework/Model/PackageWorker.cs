using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;
using System.Timers;
using System.Net.NetworkInformation;
using System.Threading;
using PacketDotNet;

namespace CourseWork.Model
{
    public class PackageWorker
    {
        /// <summary>
        /// When true the background thread will terminate
        /// </summary>
        /// <param name="args">
        /// A <see cref="string"/>
        /// </param>
        private static bool BackgroundThreadStop = false;

        /// <summary>
        /// Object that is used to prevent two threads from accessing
        /// PacketQueue at the same time
        /// </summary>
        /// <param name="args">
        /// A <see cref="string"/>
        /// </param>
        private static readonly object QueueLock = new object();

        /// <summary>
        /// The queue that the callback thread puts packets in. Accessed by
        /// the background thread when QueueLock is held
        /// </summary>
        private static List<RawCapture> PacketQueue = new List<RawCapture>();

        /// <summary>
        /// The last time PcapDevice.Statistics() was called on the active device.
        /// Allow periodic display of device statistics
        /// </summary>
        /// <param name="args">
        /// A <see cref="string"/>
        /// </param>
        private static DateTime LastStatisticsOutput = DateTime.Now;

        /// <summary>
        /// Interval between PcapDevice.Statistics() output
        /// </summary>
        /// <param name="args">
        /// A <see cref="string"/>
        /// </param>
        private static TimeSpan LastStatisticsInterval = new TimeSpan(0, 0, 2);

        private static List<Package> packages = new List<Package>();
        public static List<Package> GetSomePackageInfo(Interface interfaces)
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            string bufferName;
            string bufferMacAddress;
            
            List<Interface> allInterfaces = new List<Interface>();
            foreach (ILiveDevice dev in devices)
            {
                bufferName = dev.Name;
                bufferMacAddress = dev.MacAddress.ToString();
                if((interfaces.MacAddress == bufferMacAddress)&&(interfaces.Name == bufferName) )
                {

                    ICaptureDevice selectedDevice = dev;
                    List<Package> packages = new List<Package>();

                    var backgroundThread = new System.Threading.Thread(BackgroundThread);
                    backgroundThread.Start();

                    using var device = dev;

                    // Register our handler function to the 'packet arrival' event
                    dev.OnPacketArrival += (sender, e) => packages.Add(device_OnPacketArrival(e.GetPacket(), packages.Count(), interfaces));

                    // Open the device for capturing
                    device.Open();

                    // Start the capturing process
                    device.StartCapture();

                    // Wait for 'Enter' from the user. We pause here until being asked to
                    // be terminated

    

                    // Stop the capturing process
                    device.StopCapture();

             

                    // ask the background thread to shut down
                  

                    // wait for the background thread to terminate
                    backgroundThread.Join();

                    return packages;
                }
            }
            return null;
           

        }

        private static void BackgroundThread()
        {
            while (!BackgroundThreadStop)
            {
                bool shouldSleep = true;

                lock (QueueLock)
                {
                    if (PacketQueue.Count != 0)
                    {
                        shouldSleep = false;
                    }
                }

                if (shouldSleep)
                {
                    System.Threading.Thread.Sleep(250);
                }
                else // should process the queue
                {
                    List<RawCapture> ourQueue;
                    lock (QueueLock)
                    {
                        // swap queues, giving the capture callback a new one
                        ourQueue = PacketQueue;
                        PacketQueue = new List<RawCapture>();
                    }

                   

                    foreach (var packet in ourQueue)
                    {
                        var time = packet.Timeval.Date;
                        var len = packet.Data.Length;
                        
                    }

                    // Here is where we can process our packets freely without
                    // holding off packet capture.
                    //
                    // NOTE: If the incoming packet rate is greater than
                    //       the packet processing rate these queues will grow
                    //       to enormous sizes. Packets should be dropped in these
                    //       cases
                }
            }
        }

        private static Package device_OnPacketArrival(RawCapture rawPacket, int packageCount, Interface @interface)
        {
            if (packageCount <= 40)
            {
                PacketDotNet.Packet packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
                packet.ToString();
                PacketDotNet.IPPacket ip = packet.Extract<PacketDotNet.IPPacket>();
               string something = "Информация по пакету";
                if (packet is PacketDotNet.EthernetPacket eth)
                {
                    something = eth.ToString();
                }
                Package newPackage;
                DateTime dateTime = new DateTime();
                if (ip == null)
                    newPackage = new Package("0.0.0.0", something, dateTime.Date);
                else
                    newPackage = new Package(ip.SourceAddress.ToString(), something, dateTime.Date);

                return newPackage;
            }
            else
            {
                CaptureDeviceList devices = CaptureDeviceList.Instance;
                string bufferName;
                string bufferMacAddress;

                List<Interface> allInterfaces = new List<Interface>();
                foreach (ILiveDevice dev in devices)
                {
                    bufferName = dev.Name;
                    bufferMacAddress = dev.MacAddress.ToString();
                    if ((@interface.MacAddress == bufferMacAddress) && (@interface.Name == bufferName))
                    {
                        ICaptureDevice selectedDevice = dev;
                        dev.StopCapture();
                        dev.Close();
                        return null;
                    }
                    
                }
                BackgroundThreadStop = true;
                return null;
            }
        }
        public static List<Interface> GetAllInterfaces()
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            string bufferName;
            string bufferMacAddress;
            List<Interface> interfaces = new List<Interface>();
            foreach (ILiveDevice dev in devices)
            {
                bufferName = dev.Name;
                bufferMacAddress = dev.MacAddress.ToString();
                interfaces.Add(new Interface(bufferName, bufferMacAddress));
            }
            return interfaces;
        }

    }
}
