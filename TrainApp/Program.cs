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
            //instansierar alla listor och tilldelar dem, vardera returnerade object-värdet. 
            List<Train> trains = Train.GetTrains();
            List<Station> stations = Station.GetStations();
            List<Passenger> passengers = Passenger.GetPassenger();
            List<Schedule> schedules = Schedule.GetSchdules();
            List<TrainTrack> trainTracks = TrainTrack.GetTrainTracks();

            //(här skulle instanserna av trackobject klasserna kunna vara i framtiden)


            Console.WriteLine("Starting the Travel Plan");
            TravelPlan travelPlan = new TravelPlan(trainTracks, stations, passengers).SetTrain(trains).SetSchedule(schedules).Start();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You Exited");
        }
    }
}
