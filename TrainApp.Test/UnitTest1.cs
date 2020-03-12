using System;
using Xunit;
using TrainApp;
using System.Collections.Generic;
using System.Threading;

namespace TrainApp.Test
{
    public class UnitTest1
    {
        //[Fact]
        //public void Test1()
        //{
        //    //ORM orm = new ORM();
        //    //var x  = orm.GetTrain();

        //    Assert.NotNull(x);
        //    Assert.Equal(4, x.Count);
        //}

        [Fact]
        public void TravelPlannerTest()
        {
            List<Train> trains = new List<Train>();
            trains.Add(new Train(1, "Olle", 1, 1 ));


            List<Schedule> schedules = new List<Schedule>();
            schedules.Add(new Schedule() { TrainId = 1, StationId = 1, DepartureTime = new TimeSpan(10, 30, 00) });
            schedules.Add(new Schedule() { TrainId = 1, StationId = 2, ArrvialTime = new TimeSpan(10,40,00) , DepartureTime = new TimeSpan(10, 50, 00) });
            schedules.Add(new Schedule() { TrainId = 1, StationId = 3, ArrvialTime = new TimeSpan(11, 00, 00)});


            List<TrainTrack> trainTracks = new List<TrainTrack>();
            trainTracks.Add(new TrainTrack()
            {
                Id = 1,
                StartStationID = 1,
                CrossingDistance = 5,
                Swith1Distance = 10,
                Swith1Direction = true,
                MiddleStationID = 2,
                MiddleStationDistance = 20,
                Switch2Distance = 30,
                Swith2Direction = true,
                EndStationDistance = 50,
                EndStationID = 3
            });

          //  TravelPlan travelPlan = new TravelPlan(trainTracks, stations).Start();

            Thread.Sleep(60000);
        }
    }
}
