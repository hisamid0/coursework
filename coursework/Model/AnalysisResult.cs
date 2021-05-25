using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Model
{
    public class AnalysisResult : IEquatable<AnalysisResult>
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string Protocol { get; set; }
        public string SourcePort { get; set; }
        public string DestinationPort { get; set; }
        public string SourceHardwareAddress { get; set; }
        public string DestinationHardwareAddress { get; set; }
        public string ThreatLevel { get; set; }
        public string NecessaryMeasures { get; set; }
        public AnalysisResult(string ipaddress, string protocol, string sourcePort, string destinationPort, 
            string sourceHardwareAddress, string destinationHardwareAddress)
        {
            IpAddress = ipaddress;
            Protocol = protocol;
            SourcePort = sourcePort;
            DestinationPort = destinationPort;
            SourceHardwareAddress = sourceHardwareAddress;
            DestinationHardwareAddress = destinationHardwareAddress;
        }
        public AnalysisResult(string ipaddress, string protocol, string sourcePort, string destinationPort, 
            string sourceHardwareAddress, string destinationHardwareAddress, string threatLevel, string necessaryMeasures)
        {
            IpAddress = ipaddress;
            Protocol = protocol;
            SourcePort = sourcePort;
            DestinationPort = destinationPort;
            SourceHardwareAddress = sourceHardwareAddress;
            DestinationHardwareAddress = destinationHardwareAddress;
            ThreatLevel = threatLevel;
            NecessaryMeasures = necessaryMeasures;
        }
        public AnalysisResult()
        {

        }

        public bool Equals(AnalysisResult analysisResult)
        {
            if(Object.ReferenceEquals(analysisResult, null)) return false;
            if(Object.ReferenceEquals(this, analysisResult)) return true;
            return IpAddress.Equals(analysisResult.IpAddress) && Protocol.Equals(analysisResult.Protocol);
        }
        public override int GetHashCode()
        {
            int hashPackageIpAddress = IpAddress == null ? 0 : IpAddress.GetHashCode();
            int hashPackageProtocol = Protocol == null ? 0 : Protocol.GetHashCode();
            return hashPackageIpAddress ^ hashPackageProtocol;
        }
    }
}
