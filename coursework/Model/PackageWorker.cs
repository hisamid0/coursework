﻿using System;
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

                    dev.OnPacketArrival += (sender, e) => packages.Add(device_OnPacketArrival(e.GetPacket()));
                    dev.Open();
                    dev.Capture();
                    dev.Close();
                    return packages;
                }
            }
            return null;
           

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
