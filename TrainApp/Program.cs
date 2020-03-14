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
            List<Train> trains = Train.GetTrains();
            List<Station> stations = Station.GetStations();
            List<Passenger> passengers = Passenger.GetPassenger();
            List<Schedule> schedules = Schedule.GetSchdules();
            List<TrainTrack> trainTracks = TrainTrack.GetTrainTracks();

            //instanciate trackobjects
            Console.WriteLine("Starting the Travel Plan");
            TravelPlan travelPlan = new TravelPlan(trainTracks, stations, passengers).SetTrain(trains).SetSchedule(schedules).Start();
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();

            Console.WriteLine("exit");
        }
    }
}
