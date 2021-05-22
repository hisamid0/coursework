﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class Package
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Protocol { get; set; }
        public string SourceHardwareAddress { get; set; }
        public string DestinationHardwareAddress { get; set; }
        public string PayLoadData { get; set; }

        public DateTime Date { get; set; }

        public Package(string ipaddress,string protocol, string sourceHardwareAddress, DateTime date, string destinationHardwareAddress,string payLoadData)
        {
            IpAddress = ipaddress;
            Protocol = protocol;
            SourceHardwareAddress = sourceHardwareAddress;
            DestinationHardwareAddress = destinationHardwareAddress;
            PayLoadData = payLoadData;
            Date = date;
        }
        public Package()
        {

        }

    }
}
