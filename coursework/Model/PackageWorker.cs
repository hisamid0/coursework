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

        private static List<Package> packages = new List<Package>();
        public static List<Package> GetSomePackageInfo(Interface interfaces)
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            string bufferName;
            string bufferMacAddress;
            
            List<Interface> allInterfaces = new List<Interface>();
            foreach (ICaptureDevice dev in devices)
            {
                dev.Open();
                bufferName = dev.Name;
                bufferMacAddress = dev.MacAddress.ToString();
                if((interfaces.MacAddress == bufferMacAddress)&&(interfaces.Name == bufferName) )
                {
                    while (packages.Count() < 500)
                    {
                        ICaptureDevice selectedDevice = dev;
                        //List<Package> packages = new List<Package>();
                        //packages.Add(new Package("255.255.255.255", "someinfo", DateTime.Now));
                        int readTimeoutMilliseconds = 100;
                        dev.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                        
                        RawCapture rawCapture = null;
                        rawCapture = dev.GetNextPacket();
                        if (rawCapture == null)
                            continue;
                        PacketDotNet.Packet packet = PacketDotNet.Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
                        packet.ToString();
                        PacketDotNet.IPPacket ip = packet.Extract<PacketDotNet.IPPacket>();
                        string sourceHardwareAddress = "";
                        string destinationHardwareAddress = "";
                        string payLoadData = "";
                        string destinationPort = "";
                        string sourcePort = "";
                        var tcp = packet.Extract<PacketDotNet.TcpPacket>();
                        var udp = packet.Extract<PacketDotNet.UdpPacket>();
                        if(udp != null)
                        {
                            destinationPort = udp.DestinationPort.ToString();
                            sourcePort = udp.SourcePort.ToString();
                        }
                        if (tcp != null) 
                        {
                            destinationPort = tcp.DestinationPort.ToString();
                            sourcePort = tcp.SourcePort.ToString();
                        }
                        if (packet is PacketDotNet.EthernetPacket eth)
                        {
                            sourceHardwareAddress = eth.SourceHardwareAddress.ToString();
                            destinationHardwareAddress = eth.DestinationHardwareAddress.ToString();
                            if(eth.BytesSegment != null)
                                payLoadData = eth.BytesSegment.ToString();
                        }
                        Package newPackage;
                        DateTime dateTime = new DateTime();
                        if (ip == null)
                            newPackage = new Package("0.0.0.0","???","?????","?????", sourceHardwareAddress, dateTime.Date,destinationHardwareAddress,payLoadData);
                        else
                            newPackage = new Package(ip.SourceAddress.ToString(),ip.Protocol.ToString(),sourcePort,destinationPort, sourceHardwareAddress, dateTime.Date,destinationHardwareAddress,payLoadData);
                        
                        dev.Close();
                        packages.Add(new Package(newPackage.IpAddress,newPackage.Protocol,sourcePort,destinationPort, newPackage.SourceHardwareAddress, DateTime.Now,destinationHardwareAddress,payLoadData));
                    }

                    return packages;
                    //dev.Capture();


                   
                    
                }
                dev.Close();
            }
            return null;
           

        }



        public static List<Interface> GetAllInterfaces()
        {
            CaptureDeviceList devices = CaptureDeviceList.Instance;
            string bufferName;
            string bufferMacAddress;
            List<Interface> interfaces = new List<Interface>();
            foreach (ICaptureDevice dev in devices)
            {
                bufferName = dev.Name;
                dev.Open();
                bufferMacAddress = dev.MacAddress.ToString();
                interfaces.Add(new Interface(bufferName, bufferMacAddress));
                dev.Close();
            }
            return interfaces;
        }

        public static List<AnalysisResult> GetAllUniquesIpMac(List<Package> packages)
        {
            List<AnalysisResult> analysisResults = new List<AnalysisResult>();
            foreach (Package pack in packages)
            {
                analysisResults.Add(new AnalysisResult(pack.IpAddress, pack.Protocol, pack.SourcePort, pack.DestinationPort, pack.SourceHardwareAddress, pack.DestinationHardwareAddress));
            }
            
            return analysisResults;
        }

        public static List<AnalysisResult> PackageAnalyzer(List<AnalysisResult> analysisResults)
        {
            foreach (var aRes in analysisResults)
            {
                aRes.ThreatLevel = "5";                
            }

            return analysisResults.Distinct().ToList();   

        }

    }
}
