using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainApp
{
    public class TrainTrack
    {
        public int Id { get; private set; }
        public int StartStationId { get; private set; }
        public int CrossingPosition { get; private set; }
        public int Switch1Position { get; private set; }
        public bool Switch1Left { get; set; }
        public int MiddleStationId { get; private set; }
        public int MiddleStationPosition { get; private set; }
        public int Switch2Position { get; private set; }
        public bool Switch2Left { get; set; }
        public int EndStationId { get; private set; } 
        public int EndStationPosition { get; private set; }

        public TrainTrack(int id, int startStationId, int crossingPosition, int switch1Pos, bool switch1Left, int midStationId, int midStationPos, int switch2Pos, bool switch2Left, int endStationId, int endStationPos)
        {
            Id = id;
            StartStationId = startStationId;
            CrossingPosition = crossingPosition;
            Switch1Position = switch1Pos;
            Switch1Left = switch1Left;
            MiddleStationId = midStationId;
            MiddleStationPosition = midStationPos;
            Switch2Position = switch2Pos;
            Switch2Left = switch2Left;
            EndStationId = endStationId;
            EndStationPosition = endStationPos;
        }
        public static List<TrainTrack> GetTrainTracks()
        {
            string line;
            List<TrainTrack> listOfTrainTracks = new List<TrainTrack>();
            StreamReader file = new StreamReader(@"./Data/traintracks.txt");

            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] lines = line.Split(',');
                listOfTrainTracks.Add(new TrainTrack(int.Parse(lines[0]), int.Parse(lines[1]), int.Parse(lines[2]), int.Parse(lines[3]), bool.Parse(lines[4]), int.Parse(lines[5]), int.Parse(lines[6]), int.Parse(lines[7]), bool.Parse(lines[8]), int.Parse(lines[9]), int.Parse(lines[10])));
            }

            file.Close();
            return listOfTrainTracks;
        }
    }
}
