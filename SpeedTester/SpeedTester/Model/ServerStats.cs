using System;

namespace SpeedTester.Model
{
   public  class ServerStats
    {
        public int DataSize { get; set; } = 0;
        public int TotalSize { get; set; } = 0;
        public int TransmissionTime { get; set; } = 0;
        public int TransmissionSpeed { get; set; } = 0;
        public int LostData { get; set; } = 0;
        public int TransmissionError { get; set; } = 0;
        public ServerStats ShallowCopy()
        {
            return (ServerStats)this.MemberwiseClone();
        }
    }
}
