using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TrainApp
{
    public class Passenger
    {
        public int Id { get; set; }
        public string PassengerName { get; set; }
        public Passenger(int id, string passengerName)
        {
            Id = id;
            PassengerName = passengerName;

        }

        public static List<Passenger> GetPassenger()
        {
            string line;
            List<Passenger> listOfPassengers = new List<Passenger>();
            StreamReader file =
                new StreamReader(@"./Data/passengers.txt");

            while ((line = file.ReadLine()) != null)
            {
                string[] words = line.Split(';');
                listOfPassengers.Add(new Passenger(int.Parse(words[0]), words[1]));
            }

            file.Close();
            return listOfPassengers;
        }
    }
}
