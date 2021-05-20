using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;
using System.Timers;
using System.Net.NetworkInformation;

namespace CourseWork.Model
{
    public class PackageWorker
    {

        private static List<Package> packages = new List<Package>();
        public static List<Package> GetSomePackageInfo(ICaptureDevice device)
        {
            Timer timer = new Timer();
            ICaptureDevice selectedDevice = device;
            List<Package> packages = new List<Package>();

            device.OnPacketArrival += (sender, e)=> packages.Add(device_OnPacketArrival(e.GetPacket()));
            device.Open();
            device.Capture();
            device.Close();
            return packages;

        }


        private static Package device_OnPacketArrival(RawCapture rawPacket)
        {
            PacketDotNet.Packet packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            packet.ToString();
            PacketDotNet.IPPacket ip = packet.Extract<PacketDotNet.IPPacket>();
            string something= "Информация по пакету";
            if (packet is PacketDotNet.EthernetPacket eth)
            {
                something = eth.ToString();
            }
            DateTime dateTime = new DateTime();
            Package newPackage = new Package(ip.SourceAddress.ToString(),something,dateTime.Date.Date);

            return newPackage;
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
