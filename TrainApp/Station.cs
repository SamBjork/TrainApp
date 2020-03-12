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
        public Station(int id, string stationName, bool endStation)
        {
            ID = id;
            StationName = stationName;
            EndStation = endStation;
        }


        public static List<Station> GetStation()
        {
            string line;
            List<Station> listOfStations = new List<Station>();
            StreamReader file =
                new StreamReader(@"./Data/stations.txt");

            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split('|');
                listOfStations.Add(new Station(int.Parse(words[0]), words[1], bool.Parse(words[2])));
            }

            file.Close();
            return listOfStations;
        }
    }
}

