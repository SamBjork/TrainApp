using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainApp
{
    public class Station
    {
        public int ID { get; private set; }
        public string StationName { get; private set; }
        public bool EndStation { get; private set; }
        public List<Passenger> PassengersInStation { get; set; }
        public Station(int id, string stationName, bool endStation)
        {
            ID = id;
            StationName = stationName;
            EndStation = endStation;
            PassengersInStation = new List<Passenger>();
        }

        public static List<Station> GetStations()
        {
            string line;
            List<Station> listOfStations = new List<Station>();
            StreamReader file = new StreamReader(@"./Data/stations.txt");

            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] lines = line.Split('|');
                listOfStations.Add(new Station(int.Parse(lines[0]), lines[1], bool.Parse(lines[2])));
            }

            file.Close();
            return listOfStations;
        }
    }
}

