using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainApp
{
    public class ORM
    {
        string path = "./Data/";
        public List <Train> GetTrain()
        {
            List<Train> trains = new List<Train>();
            var filePath = "trains.txt";
            var fileLines = LoadFileLines(filePath, ',');

            for (int i = 0; i < fileLines.Count; i++)
            {
                string[] lineColumns = fileLines[i];

                Train train = new Train();
                train.ID = Convert.ToInt32(lineColumns[0]);
                train.Name = lineColumns[1];
                train.MaxSpeed = Convert.ToInt32(lineColumns[2]);
                train.Operated = Convert.ToBoolean(lineColumns[3]);

                trains.Add(train);
            }
            return trains; 
        }

        public List<Station> GetStation()
        {
            List<Station> stations = new List<Station>();
            var filePath = "trains.txt";
            var fileLines = LoadFileLines(filePath, ',');

            for (int i = 0; i < fileLines.Count; i++)
            {
                string[] lineColumns = fileLines[i];

                Station station = new Station();
            }
            return stations;
        }

        private List<string[]> LoadFileLines(string filePath, char splitter)
        {
            List<string[]> returnList = new List<string[]>();

            string[] lines = File.ReadAllLines(path + filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                    continue;

                string[] line = lines[i].Split(splitter);

                returnList.Add(line);
            }
            return returnList;
        }
    }
}
