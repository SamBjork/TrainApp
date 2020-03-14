using System;
using Xunit;
using TrainApp;
using System.Collections.Generic;
using System.Threading;

namespace TrainApp.Tests
{
    public class TrainTests
    {
        [Fact]
        public void TrainTest_ReadTrain_GetTrain()
        {
            var x = Train.GetTrains();

            Assert.NotNull(x);
            Assert.Equal(4, x.Count);
        }

        [Fact]
        public void TravelPlannerTest()
        {
            List<Train> trains = new List<Train>();
            trains.Add(new Train(1, "Olle", 1, 1 ));


            List<Schedule> schedules = Schedule.GetSchdules();

          //  TravelPlan travelPlan = new TravelPlan(trainTracks, stations).Start();

        }
    }
}
