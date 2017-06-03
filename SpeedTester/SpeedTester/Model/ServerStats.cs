using System;

namespace SpeedTester.Model
{
   public  class ServerStats
    {
        public String TCPDataSize { get; set; } = "0";
        public String TCPTotalSize { get; set; } = "0";
        public String TCPTransmissionTime { get; set; } = "0";
        public String TCPTransmissionSpeed { get; set; } = "0";
        public String TCPLostData { get; set; } = "0";
        public String TCPTransmissionError { get; set; } = "0";
        public String UDPDataSize { get; set; } = "0";
        public String UDPTotalSize { get; set; } = "0";
        public String UDPTransmissionTime { get; set; } = "0";
        public String UDPTransmissionSpeed { get; set; } = "0";
        public String UDPLostData { get; set; } = "0";
        public String UDPTransmissionError { get; set; } = "0";
    }
}
