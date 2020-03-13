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
        List<Passenger> _passengers;

        TimeSpan globalClock = new TimeSpan(10, 15, 00);
        TimeSpan zero = new TimeSpan(00, 00, 00);

        public TravelPlan(List<TrainTrack> trainTracks, List<Station> stations, List<Passenger> passengers)
        {
            _trainTracks = trainTracks;
            _stations = stations;
            _passengers = passengers;
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
                    Thread.Sleep(1000);
                    globalClock = globalClock.Add(new TimeSpan(00, 01, 00));
                    foreach (var train in _trains)
                    {
                        var track = _trainTracks.Where(x => x.Id == train.TrainTrackId).FirstOrDefault();
                        var trainTimeTableList = _schedules.Where(x => x.TrainId == train.ID).ToList();


                        switch (train.trainState)
                        {
                            case TrainState.atStartStation:

                                train.Stop();

                                var timeStartStation = trainTimeTableList.Where(x => x.StationId == track.StartStationID).FirstOrDefault();

                                Console.WriteLine("The train " + train.Name + " is at: " + _stations.Where(x => x.ID == track.StartStationID).Single().StationName + " station");

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

                                if (train.DistanceTravelled >= track.Switch1Distance)
                                {
                                    track.Switch1Direction = false;
                                    Console.WriteLine("First Switch is switched to it's left position");
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
                                    train.trainState = TrainState.onWayToSecondSwitch;
                                }
                                break;

                            case TrainState.onWayToSecondSwitch:
                                if (train.DistanceTravelled >= track.Switch2Distance)
                                {
                                    track.Switch2Direction = true;
                                    Console.WriteLine("Second Switch is switched to it's right position");
                                    train.trainState = TrainState.onWayToEndStation;
                                }
                                break;

                            case TrainState.onWayToEndStation:
                                if (train.DistanceTravelled >= track.EndStationDistance)
                                {
                                    train.trainState = TrainState.atEndStation;
                                    train.Stop();
                                    Console.WriteLine("The train has arrived at: " + _stations.Where(x => x.ID == track.EndStationID).Single().StationName);
                                }

                                break;

                            case TrainState.atEndStation:

                                break;
                        }
                    }
                }
            }
            return this;
        }
    }
}
