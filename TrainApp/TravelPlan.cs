using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using TrainApp;

namespace TrainApp
{
    public class TravelPlan
    {
        List<Train> _trains;
        List<TrainTrack> _trainTracks;
        List<Schedule> _schedules;
        List<Station> _stations;

        TimeSpan globalClock = new TimeSpan(10, 25, 00);
        TimeSpan zero = new TimeSpan(00, 00, 00);

        public TravelPlan(List<TrainTrack> trainTracks, List<Station> stations)
        {
            _trainTracks = trainTracks;
            _stations = stations;
        }

        public TravelPlan SetTrain(List<Train> trains)
        {
            _trains = trains;
            return this;
        }

        public TravelPlan SetSchedule(List<Schedule> schedules)
        {
            _schedules = schedules;
            return this;
        }

        public TravelPlan Start()
        {

            Thread planerthread = new Thread(Depart);
            planerthread.IsBackground = true;
            planerthread.Start();


            void Depart()
            {
                while (true)
                {
                    Thread.Sleep(500);
                    globalClock = globalClock.Add(new TimeSpan(00, 01, 00));
                    Console.WriteLine("Clock is: " + globalClock);
                    foreach (var train in _trains)
                    {
                        var track = _trainTracks.Where(x => x.Id == train.TrainTrackId).FirstOrDefault();
                        var trainTimeTableList = _schedules.Where(x => x.TrainId == train.ID).ToList();


                        switch (train.trainState)
                        {
                            case TrainState.atStartStation:

                                train.Stop();

                                var timeStartStation = trainTimeTableList.Where(x => x.StationId == track.StartStationID).FirstOrDefault();
                                
                                if ((timeStartStation.DepartureTime - globalClock) >= zero)
                                    Console.WriteLine("Train waiting to depart. Departing in : " + (timeStartStation.DepartureTime - globalClock));

                                if (globalClock >= timeStartStation.DepartureTime)
                                {
                                    Console.WriteLine("Train Departing");
                                    train.Start();
                                    train.trainState = TrainState.onWayToClosingCrossing;
                                }
                                break;

                            case TrainState.onWayToClosingCrossing:

                                if (train.DistanceTravelled >= track.CrossingDistance - 1)
                                {
                                    Console.WriteLine("Closing Crossing");
                                    train.trainState = TrainState.onWayToOpenCrossing;
                                }
                                break;

                            case TrainState.onWayToOpenCrossing:
                                if (train.DistanceTravelled >= track.CrossingDistance + 1)
                                {
                                    Console.WriteLine("Opening Crossing");
                                    train.trainState = TrainState.onWayToFirstSwitch;
                                }
                                break;

                            case TrainState.onWayToFirstSwitch:

                                if (train.DistanceTravelled >= track.Swith1Distance)
                                {
                                    track.Swith1Direction = true;
                                    Console.WriteLine("First Switch is switched to it's right position");
                                    train.trainState = TrainState.onWayToMiddleStation;
                                }
                                break;

                            case TrainState.onWayToMiddleStation:

                                if (train.DistanceTravelled >= track.MiddleStationDistance)
                                {
                                    train.trainState = TrainState.atMiddleStation;
                                    train.Stop();
                                    Console.WriteLine("The train has arrived at: " + _stations.Where(x => x.ID == track.MiddleStationID).Single().StationName);
                                }
                                break;

                            case TrainState.atMiddleStation:
                                var timeMiddleStation = trainTimeTableList.Where(x => x.StationId == track.MiddleStationID).Single();

                                if ((timeMiddleStation.DepartureTime - globalClock) >= zero)
                                    Console.WriteLine("Train waiting to depart. Departing in : " + (timeMiddleStation.DepartureTime - globalClock));

                                else
                                {
                                    Console.WriteLine("Train Departing");
                                    train.Start();
                                    train.trainState = TrainState.onWayToEndStation;
                                }
                                break;

                            case TrainState.onWayToEndStation:


                                break;

                            case TrainState.atEndStation:

                                break;
                        }



                    }

                    // check distance of all trains
                    // var traindistance = trains[0].Distance;
                    //if train disance == destination stop train
                    //if all trains are stoped, break
                    //Console.WriteLine(traindistance);
                }

            }
            return this;
        }
    }
}
