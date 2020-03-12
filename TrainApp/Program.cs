using System;
using System.Collections.Generic;
using System.Threading;

namespace TrainApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TravelPlannerTest();
        }
        public static void TravelPlannerTest()
        {
            List<Train> trains = new List<Train>();
            trains.Add(new Train(1, "Olle", 1, 1));

            List<Station> stations = Station.GetStation();

            List<Schedule> schedules = new List<Schedule>();
            schedules.Add(new Schedule() { TrainId = 1, StationId = 1, DepartureTime = new TimeSpan(10, 30, 00) });
            schedules.Add(new Schedule() { TrainId = 1, StationId = 2, ArrvialTime = new TimeSpan(10, 50, 00), DepartureTime = new TimeSpan(11, 00, 00) });
            schedules.Add(new Schedule() { TrainId = 1, StationId = 3, ArrvialTime = new TimeSpan(11, 30, 00) });


            List<TrainTrack> trainTracks = new List<TrainTrack>();
            trainTracks.Add(new TrainTrack()
            {
                Id = 1,
                StartStationID = 1,
                CrossingDistance = 5,
                Swith1Distance = 10,
                Swith1Direction = false,
                MiddleStationID = 2,
                MiddleStationDistance = 20,
                Switch2Distance = 30,
                Swith2Direction = true,
                EndStationDistance = 50,
                EndStationID = 3
            });
            Console.WriteLine("starting Travel Plan ");
            TravelPlan travelPlan = new TravelPlan(trainTracks, stations).SetTrain(trains).SetSchedule(schedules).Start();
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();

            Console.WriteLine("exit");
        }
    }
}
