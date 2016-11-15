using System;
using System.Collections.Generic;
using System.Linq;
using PriorityQueue_Wiener;
using System.Text;
using System.Threading.Tasks;

namespace ConventionRegistration
{
    class ConventionRegistration
    {
        private const int NumPatrons = 100;                 //numer of patrons
        private static Random ran = new Random();           //uniform random number generator
        private static PriorityQueue<Evnt> PQ;              //Priority queue of events
        private static DateTime openTime;                   //Time the convention opens
        private static int maxPresent = 0;                  //The largest number present during the simulation
        private static TimeSpan shortest,                   //The shortest windowTime
                                longest,                    //The longest windowTime
                                totalTime;                    //The total windowTime
                                //avgTime;                      //The average windowTime
        static void MainSimulation()
        {
            PQ = new PriorityQueue<Evnt>();
            openTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 7, 0, 0);

            GeneratePatronEvents();
            DoSimulation();
            ShowStatistics();


        }

        

        private static void GeneratePatronEvents()
        {
            TimeSpan start;
            TimeSpan interval;
            shortest = new TimeSpan(0, 100000, 0);  //shortest windowTime
            longest = new TimeSpan(0, 0, 0);        //longest windowTime
            //avgTime = TODO implement me

            for (int patron = 1; patron <= NumPatrons; patron++)
            {
                //Random start time based on the number of minutes in the 10 hours we are open
                start = new TimeSpan(0, ran.Next(10 * 60), 0); //TODO: this is totally not right for P4
                //Random (neg. exp.) interval with a min of 1.5 minutes; expected time = 4.5minutes
                interval = new TimeSpan(0, 0, 0, 0, (int)NegExp(270000.0, 90000.0));
                totalTime += interval;
                double test = totalTime.Ticks;

                if (shortest > interval)        //remember the shortest windowTime
                    shortest = interval;

                if (longest < interval)         //remember the longest windowTime
                    longest = interval;

                //Enqueue the arrival event for this person
                //PQ.Enqueue(new Evnt(EVENTTYPE.ENTER, openTime.Add(start), patron));

                //Enqueue the departure event for this person
                //PQ.Enqueue(new Evnt(EVENTTYPE.LEAVE, openTime.Add(start + interval), patron));
            }

            //Calculate and display the average time patrons spend at the window
            //utility.skip (is this the sleep function?)
            int seconds = (int)(totalTime.TotalSeconds / NumPatrons);
            TimeSpan avgTime = new TimeSpan(0, 0, seconds);

            Console.WriteLine($"The average time people spent in line was {avgTime}");
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();

        }

        private static void DoSimulation()
        {
            int lineCount = 0;
            maxPresent = 0;
            int current = 0;

            while (PQ.Count > 0)
            {
                //TODO console output here maybe

                if (PQ.Peek().Type == EVENTTYPE.ENTER)
                {
                    current++;
                    if (current > maxPresent)
                        maxPresent = current;
                }
                else
                    current--;

                //output stuff here

                PQ.Dequeue();
                //if((lineCount % 30) == 0)
                    //utility press a key
            }
        }

        private static void ShowStatistics()
        {
            throw new NotImplementedException();
        }


        public static int GetShortestLine(List<Queue<Registrants>> QueueList)
        {
            int ShortestLineFound = 0;
            int min = 1000;
            int temp;
            int index = 0;
            int i = 0;
            foreach (Queue<Registrants> c in QueueList)
            {
                temp = c.Count();

                if (temp < min)
                {
                    min = temp;
                    index = i;
                    i++;
                }
            }
            return ShortestLineFound = index;
        }

        public static int ShortestLine(List<Queue<Registrants>> listOfQs)
        {
            int smallestCount = 1000;
            int indexOfQueue = -1;

            for (int i = 0; i < listOfQs.Count; i++)
            {
                if (listOfQs[i].Count < smallestCount)
                {
                    smallestCount = listOfQs[i].Count;
                    indexOfQueue = i;
                }
            }

            return indexOfQueue;
        }

        public static int LongestLine(List<Queue<Registrants>> listOfQs)
        {
            int biggestCount = 0;

            for (int i = 0; i < listOfQs.Count; i++)
            {
                if (listOfQs[i].Count > biggestCount)
                {
                    biggestCount = listOfQs[i].Count;
                }
            }

            return biggestCount;
        }

        private static double NegExp(double ExpectedValue, double minimum)
        {
            return (-(ExpectedValue - minimum) * Math.Log(ran.NextDouble(), Math.E) + minimum);
        }

    }//end class
}//end namespace
