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
        public int MaxSpeed { get; private set; }
        public bool Operated { get; private set; }
        public int TrainTrackId { get; private set; }
        public TrainState trainState { get; set; }
        private int _distanceTravelled;
        public int DistanceTravelled {
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
        public Train(int id, string name, int maxSpeed, int trainTrackId)
        {
            ID = id;
            Name = name;
            MaxSpeed = maxSpeed;
            TrainTrackId = trainTrackId;


            thread = new Thread(Drive);
            thread.IsBackground = true;
            thread.Start();
            
        }
        private Thread thread;

        public void Drive()
        {
            int timeTravelled = 0; 

            while (true)
            {
                Thread.Sleep(500);
                try
                {
                    if(Operated == true)
                    {
                        timeTravelled += 1;

                        DistanceTravelled = MaxSpeed + timeTravelled;
                        Console.WriteLine("Train travelled: " + DistanceTravelled + "km");
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

    }
}
