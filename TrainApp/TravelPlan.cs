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

            var passengerHalfCount = (passengers.Count / 2);

            for (int i = 0; i < passengerHalfCount; i++)
            {
                _stations[0].PassengersInStation.Add(passengers.ElementAt(i));                
            }

            for (int i = passengerHalfCount; i < passengers.Count; i++)
            {
                _stations[2].PassengersInStation.Add(passengers.ElementAt(i));
            }
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
                    List<Schedule> trainTimeTableList = new List<Schedule>();
                    foreach (var train in _trains)
                    {


                        trainTimeTableList = (_schedules.Where(x => x.TrainId == train.ID).ToList()); ;
                        var track = _trainTracks.Where(x => x.Id == train.TrainTrackId).Single();
                        try
                        {
                        if (train.ID == 2)
                                StateMachineTrack1(train, trainTimeTableList, track);
                            else if (train.ID == 3)
                                StateMachineTrack2(train, trainTimeTableList, track);

                        }
                        catch
                        {
                            
                        }
        
                    }
                }
            }
            return this;
        }



        private void StateMachineTrack1(Train train, List<Schedule> trainTimeTableList, TrainTrack track)
        {
            switch (train.trainState)
            {
                case TrainState.atStartStation:

                    train.PassengersInTrain.AddRange(_stations[0].PassengersInStation);
                    _stations[0].PassengersInStation.Clear();

                    train.Stop();

                    var timeStartStation = trainTimeTableList.Where(x => x.StationId == track.StartStationId).FirstOrDefault();

                    Console.WriteLine("The train " + train.Name + " is at: " + _stations.Where(x => x.ID == track.StartStationId).Single().StationName + " station");

                    if ((TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock) >= zero)
                        Console.WriteLine("Train waiting to depart. Departing in : " + (TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock));

                    if (globalClock >= TimeSpan.Parse(timeStartStation.DepartureTime))
                    {
                        Console.WriteLine("Train Departing with " + train.PassengersInTrain.Count + " passengers in train");
                        train.Start();
                        train.trainState = TrainState.onWayToClosingCrossing;
                    }
                    break;

                case TrainState.onWayToClosingCrossing:

                    if (train.DistanceTravelled >= track.CrossingPosition - 1)
                    {
                        Console.WriteLine("Closing Crossing");
                        train.trainState = TrainState.onWayToOpenCrossing;
                    }
                    break;

                case TrainState.onWayToOpenCrossing:
                    if (train.DistanceTravelled >= track.CrossingPosition + 1)
                    {
                        Console.WriteLine("Opening Crossing");
                        train.trainState = TrainState.onWayToFirstSwitch;
                    }
                    break;

                case TrainState.onWayToFirstSwitch:

                    if (train.DistanceTravelled >= track.Switch1Position)
                    {
                        track.Switch1Left = true;
                        Console.WriteLine("First Switch is switched to it's left position");
                        train.trainState = TrainState.onWayToMiddleStation;
                    }
                    break;

                case TrainState.onWayToMiddleStation:

                    if (train.DistanceTravelled >= track.MiddleStationPosition)
                    {
                        train.trainState = TrainState.atMiddleStation;
                        train.Stop();

                        var passengerCount = (train.PassengersInTrain.Count / 2);

                        for (int i = 0; i < passengerCount; i++)
                        {
                            _stations[1].PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }

                        Console.WriteLine("The train has arrived at: " + _stations.Where(x => x.ID == track.MiddleStationId).Single().StationName);
                        Console.WriteLine("Dropped of " + _stations[1].PassengersInStation.Count + " passengers at station");
                    }
                    break;

                case TrainState.atMiddleStation:
                    var timeMiddleStation = trainTimeTableList.Where(x => x.StationId == track.MiddleStationId).Single();

                    if ((TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock) >= zero)
                        Console.WriteLine("Train waiting to depart. Departing in : " + (TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock));

                    else
                    {
                        Console.WriteLine("Train Departing with " + train.PassengersInTrain.Count + " passengers");
                        train.Start();
                        train.trainState = TrainState.onWayToSecondSwitch;
                    }
                    break;

                case TrainState.onWayToSecondSwitch:
                    if (train.DistanceTravelled >= track.Switch2Position)
                    {
                        track.Switch2Left = false;
                        Console.WriteLine("Second Switch is switched to it's right position");
                        train.trainState = TrainState.onWayToEndStation;
                    }
                    break;

                case TrainState.onWayToEndStation:
                    if (train.DistanceTravelled >= track.EndStationPosition)
                    {
                        train.trainState = TrainState.atEndStation;
                        train.Stop();
                        Console.WriteLine("The train has arrived at: " + _stations.Where(x => x.ID == track.EndStationId).Single().StationName);
                    }

                    break;

                case TrainState.atEndStation:

                    break;
            }
        }

        private void StateMachineTrack2(Train train, List<Schedule> trainTimeTableList, TrainTrack track)
        {
            switch (train.trainState)
            {
                case TrainState.atStartStation:

                    train.PassengersInTrain.AddRange(_stations[0].PassengersInStation);
                    _stations[0].PassengersInStation.Clear();

                    train.Stop();

                    var timeStartStation = trainTimeTableList.Where(x => x.StationId == track.StartStationId).FirstOrDefault();

                    Console.WriteLine("The train " + train.Name + " is at: " + _stations.Where(x => x.ID == track.StartStationId).Single().StationName + " station");

                    if ((TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock) >= zero)
                        Console.WriteLine("Train waiting to depart. Departing in : " + (TimeSpan.Parse(timeStartStation.DepartureTime) - globalClock));

                    if (globalClock >= TimeSpan.Parse(timeStartStation.DepartureTime))
                    {
                        Console.WriteLine("Train Departing with " + train.PassengersInTrain.Count + " passengers in train");
                        train.Start();
                        train.trainState = TrainState.onWayToSecondSwitch;
                    }
                    break;

                case TrainState.onWayToSecondSwitch:

                    if (train.DistanceTravelled >= track.Switch2Position)
                    {
                        track.Switch2Left = true;
                        Console.WriteLine("Second Switch is switched to it's left position");
                        train.trainState = TrainState.onWayToMiddleStation;
                    }
                    break;

                case TrainState.onWayToMiddleStation:

                    if (train.DistanceTravelled >= track.MiddleStationPosition)
                    {
                        train.trainState = TrainState.atMiddleStation;
                        train.Stop();

                        var passengerCount = (train.PassengersInTrain.Count / 2);

                        for (int i = 0; i < passengerCount; i++)
                        {
                            _stations[1].PassengersInStation.Add(train.PassengersInTrain.ElementAt(0));
                            train.PassengersInTrain.RemoveAt(0);
                        }

                        Console.WriteLine("The train has arrived at: " + _stations.Where(x => x.ID == track.MiddleStationId).Single().StationName);
                        Console.WriteLine("Dropped of " + _stations[1].PassengersInStation.Count + " passengers at station");
                    }
                    break;

                case TrainState.atMiddleStation:

                    var timeMiddleStation = trainTimeTableList.Where(x => x.StationId == track.MiddleStationId).Single();

                    if ((TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock) >= zero)
                        Console.WriteLine("Train waiting to depart. Departing in : " + (TimeSpan.Parse(timeMiddleStation.DepartureTime) - globalClock));

                    else
                    {
                        Console.WriteLine("Train Departing with " + train.PassengersInTrain.Count + " passengers");
                        train.Start();
                        train.trainState = TrainState.onWayToFirstSwitch;
                    }
                    break;

                case TrainState.onWayToFirstSwitch:

                    if (train.DistanceTravelled >= track.Switch1Position)
                    {
                        track.Switch1Left = false;
                        Console.WriteLine("First Switch is switched to it's right position");
                        train.trainState = TrainState.onWayToClosingCrossing;
                    }
                    break;

                case TrainState.onWayToClosingCrossing:

                    if (train.DistanceTravelled >= track.CrossingPosition - 1)
                    {
                        Console.WriteLine("Closing Crossing");
                        train.trainState = TrainState.onWayToOpenCrossing;
                    }
                    break;

                case TrainState.onWayToOpenCrossing:
                    if (train.DistanceTravelled >= track.CrossingPosition + 1)
                    {
                        Console.WriteLine("Opening Crossing");
                        train.trainState = TrainState.onWayToEndStation;
                    }
                    break;


                case TrainState.onWayToEndStation:
                    if (train.DistanceTravelled >= track.EndStationPosition)
                    {
                        train.trainState = TrainState.atEndStation;
                        train.Stop();
                        Console.WriteLine("The train has arrived at: " + _stations.Where(x => x.ID == track.EndStationId).Single().StationName);
                    }

                    break;

                case TrainState.atEndStation:

                    break;
            }
        }
    }
}
