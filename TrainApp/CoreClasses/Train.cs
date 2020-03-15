using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace TrainApp
{
    public class Train
    {
        public int ID { get; private set; }
        public string Name { get; private set; } 
        public float MaxSpeed { get; private set; }
        public bool Operated { get; private set; }
        public List<Passenger> PassengersInTrain { get; set; }
        public int TrainTrackId { get; private set; }
        public TrainState trainState { get; set; }
        private float _distanceTravelled;
           public int timeTravelled = 0; 
        //låser distance travlled för att den inte ska kunna ändras från två håll samtidigt
        public float DistanceTravelled {
            get 
            {
                lock (this)
                {
                    return _distanceTravelled;
                }
            }
            private set 
            {
                lock (this)
                {
                    _distanceTravelled = value;
                }
            } 
        }
        // i de olika konstruktorerna skapas och starar en ny tråd som tar som argument Drive metoden, alltså skapar en tråd som kör den metoden.
        public Train()
        {
            thread = new Thread(Drive);
            thread.IsBackground = true;
            thread.Start();
        }
        public Train(int id, string name, int maxSpeed, int trainTrackId)
        {
            ID = id;
            Name = name;
            MaxSpeed = maxSpeed;
            TrainTrackId = trainTrackId;
            PassengersInTrain = new List<Passenger>();

            thread = new Thread(Drive);
            thread.IsBackground = true;
            thread.Start();
            
        }
        private Thread thread;

        //simulerar att tåget åker med extremt lätt mattematik, (för att alltså simulera att en sekund är en minut.
        //som man kan se körs bara if-satsen som faktiskt innehållelr "Kör"-logiken när operated är sant
        public void Drive()
        {

            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    if(Operated == true)
                    {
                        DistanceTravelled = (MaxSpeed/60) * timeTravelled;
                        Console.WriteLine("Train travelled: " + DistanceTravelled + "km");
                        timeTravelled ++;
                    }
                }
                catch (Exception)
                {

                    break;
                }
            }
        }
        //och det är här i start alternativt stop-metoderna vi bestämmer om tåget åker eller ej.
        internal void Start()
        { 
            Operated = true;
        }

        internal void Stop()
        {
            Operated = false;   
        }
        public static List<Train> GetTrains()
        {
            string line;
            List<Train> listOfTrains = new List<Train>();
            StreamReader file = new StreamReader(@"./Data/trains.txt");
            file.ReadLine();
            while ((line = file.ReadLine()) != null)
            {
                string[] lines = line.Split(',');
                listOfTrains.Add(new Train(int.Parse(lines[0]), lines[1], int.Parse(lines[2]), int.Parse(lines[3])));
            }
            
            file.Close();
            return listOfTrains;
        }
    }
}
