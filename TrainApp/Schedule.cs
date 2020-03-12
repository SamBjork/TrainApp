using System;
using System.Collections.Generic;
using System.Text;

namespace TrainApp
{
    public class Schedule
    {
        public int TrainId { get; set; }
        public int StationId { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public TimeSpan ArrvialTime { get; set; }
    }
}
