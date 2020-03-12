using System;
using System.Collections.Generic;
using System.Text;

namespace TrainApp
{
    public class TrainTrack
    {
        public int Id { get; set; }
        public int StartStationID { get; set; }
        public int MiddleStationDistance { get; set; }
        public int MiddleStationID { get; set; }
        public int EndStationDistance { get; set; }
        public int EndStationID { get; set; } 
        public int Switch1Distance { get; set; }
        public bool Switch1Direction { get; set; }
        public int Switch2Distance { get; set; }
        public bool Switch2Direction { get; set; }
        public int CrossingDistance { get; set; }

    }
}
