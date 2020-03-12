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

            List<Passenger> passengers = Passenger.GetPassenger();

            List<Schedule> schedules = new List<Schedule>();
            schedules.Add(new Schedule() { TrainId = 1, StationId = 1, DepartureTime = new TimeSpan(10, 20, 00) });
            schedules.Add(new Schedule() { TrainId = 1, StationId = 2, ArrvialTime = new TimeSpan(10, 43, 00), DepartureTime = new TimeSpan(10, 45, 00) });
            schedules.Add(new Schedule() { TrainId = 1, StationId = 3, ArrvialTime = new TimeSpan(10, 59, 00) });


            List<TrainTrack> trainTracks = new List<TrainTrack>();
            trainTracks.Add(new TrainTrack()
            {
                Id = 1,
                StartStationID = 1,
                CrossingDistance = 5,
                Switch1Distance = 10,
                Switch1Direction = false,
                MiddleStationID = 2,
                MiddleStationDistance = 20,
                Switch2Distance = 30,
                Switch2Direction = true,
                EndStationDistance = 50,
                EndStationID = 3
            });
            Console.WriteLine("starting Travel Plan ");
            TravelPlan travelPlan = new TravelPlan(trainTracks, stations, passengers).SetTrain(trains).SetSchedule(schedules).Start();
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();

            Console.WriteLine("exit");
        }
    }
}
