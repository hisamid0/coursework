using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.LibPcap;
using System.Timers;


namespace CourseWork.Model
{
    public class PackageWorker
    {
        

        public delegate void PacketArrivalEventHandler(object sender, PacketCapture e);
        public static List<Package> GetSomePackages(CaptureDeviceList device)
        {
            Timer timer = new Timer();
            var selectedDevice = device;
            selectedDevice.
            
        }

        public static List<Interface> GetAllInterfaces()
        {
            var devices = CaptureDeviceList.Instance;
            string bufferName;
            string bufferMacAddress;
            List<Interface> interfaces = new List<Interface>();
            foreach (var dev in devices)
            {
                bufferName = dev.Name;
                bufferMacAddress = dev.MacAddress.ToString();
                interfaces.Add(new Interface(bufferName, bufferMacAddress));
            }
            return interfaces;
                

        }

    }
}
