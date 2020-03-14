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
