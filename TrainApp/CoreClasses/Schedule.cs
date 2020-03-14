using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainApp
{
    public class Schedule
    {
        public int TrainId { get; private set; }
        public int StationId { get; private set; }
        public string DepartureTime { get; private set; }
        public string ArrvialTime { get; private set; }

        public Schedule(int trainId, int stationId, string departureTime, string arrivalTime)
        {
            TrainId = trainId;
            StationId = stationId;
            DepartureTime = departureTime;
            ArrvialTime = arrivalTime;
        }

        public static List<Schedule> GetSchdules()
        {
            string line;
            List<Schedule> listOfSchedules = new List<Schedule>();
            StreamReader file = new StreamReader(@"./Data/schedules.txt");

            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] lines = line.Split(',');
                listOfSchedules.Add(new Schedule(int.Parse(lines[0]), int.Parse(lines[1]), lines[2], lines[3]));
            }

            file.Close();
            return listOfSchedules;
        }
    }
}
