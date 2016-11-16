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
        private static TimeSpan shortest,                   //The shortest windowTime
                                longest,                    //The longest windowTime
                                totalTime,                  //The total windowTime
                                MaxWindowTime,              //max amount of time at the window
                                MinWindowTime;              //min amount of time at the window

        public static int testLeaveWin { get; private set; }
        public static int counterPatrons { get; private set; }
        public static int longestQ { get; private set; }

        public static TimeSpan testTime;

        public static void DoSimulation(int ExpectedRegistrants, int NumberOfHoursOpen, int NumberOfQueues, double ExpectedRegistrationTime)
        {

            TimeSpan tick = new TimeSpan(1000000);                              //tick = .1 sec
            DateTime openTime = new DateTime(2016, 11, 1, 8, 0, 0, 0);
            TimeSpan hoursOpenTimeSpan = new TimeSpan(NumberOfHoursOpen, 0, 0);
            DateTime closingTime = new DateTime(2016, 11, 1, 8, 0, 0, 0);
            closingTime += hoursOpenTimeSpan;
            MaxWindowTime = new TimeSpan(0, 0, 0, 0);
            MinWindowTime = new TimeSpan(100, 100, 100, 100);

            int actualNumRegistrants = Poisson(ExpectedRegistrants);       //actual number of registrants
            List<int> patronIdListFromPoisson = new List<int>(actualNumRegistrants);

            for (int i = 0; i < actualNumRegistrants; i++)
                patronIdListFromPoisson.Add(i);

            RandomizeList(patronIdListFromPoisson);

            TimeSpan enterConvetionTimer = new TimeSpan(hoursOpenTimeSpan.Ticks / actualNumRegistrants);     //how often people enter the convention
            double tickNumTrigger = enterConvetionTimer.Ticks / tick.Ticks;

            int numberOfTotalTicks = (int)(hoursOpenTimeSpan.Ticks / tick.Ticks);     //how often people enter the convention

            List<int> entranceTimesInTicks = new List<int>(actualNumRegistrants);
            for (int i = 0; i < actualNumRegistrants; i++)
                entranceTimesInTicks.Add(ran.Next(numberOfTotalTicks));

            entranceTimesInTicks.Sort();

            DateTime currentTime = openTime;

            List<Queue<Registrants>> listOfQs = new List<Queue<Registrants>>(NumberOfQueues);
            for (int i = 0; i < NumberOfQueues; i++)
                listOfQs.Add(new Queue<Registrants>());

            Queue<Registrants> expectedRegistrants = new Queue<Registrants>(actualNumRegistrants);

            for (int i = 0; i < actualNumRegistrants; i++)
                expectedRegistrants.Enqueue(new Registrants(patronIdListFromPoisson[i]));

            PriorityQueue<Registrants> PQ = new PriorityQueue<Registrants>();
            PriorityQueue<Evnt> PQ2 = new PriorityQueue<Evnt>();
            Evnt[] PQArr = new Evnt[NumberOfQueues];

            int failDQCounter = 0;
            testLeaveWin = 0;
            counterPatrons = 0;
            longestQ = 0;
            bool filled = false;
            int tickCounter = 0;

            while (!((currentTime > closingTime) && counterPatrons == testLeaveWin))//&& counterPatrons == testLeaveWin
            {

                while (counterPatrons != actualNumRegistrants && entranceTimesInTicks[counterPatrons] == tickCounter)
                {
                    expectedRegistrants.Peek().lineChoice = ConventionRegistration.ShortestLine(listOfQs);
                    listOfQs[expectedRegistrants.Peek().lineChoice].Enqueue(expectedRegistrants.Dequeue());

                    if (longestQ < LongestLine(listOfQs))       //save the length of longest Queue
                        longestQ = LongestLine(listOfQs);

                    counterPatrons++;
                }


                while (PQ2.Count > 0 && PQ2.Peek().Depart <= currentTime)
                {


                    int lineChoice = PQ2.Peek().LineChoice;
                    MaxWindowTime= MaxTimeAtWindow(PQ2.Peek().windowTime);
                    MinWindowTime = MinTimeAtWindow(PQ2.Peek().windowTime);

                    totalTime += PQ2.Peek().windowTime;
                  
                    PQ2.Dequeue();
                    testLeaveWin++;


                    try
                    {
                        listOfQs[lineChoice].Dequeue();                             //the patron leaves the queue (line they were waiting in)
                        PQArr[lineChoice] = null;
                    }
                    catch (Exception)
                    {
                        failDQCounter++;
                    }

                    //if there is someone next in that line, they enter the PQ (approach the window) and are assigned a wait time
                    if (listOfQs[lineChoice].Count > 0)
                    {
                        listOfQs[lineChoice].Peek().Arrival = new Evnt(currentTime, lineChoice, listOfQs[lineChoice].Peek().PatronNum);            //assign wait time

                        PQ2.Enqueue(listOfQs[lineChoice].Peek().Arrival);                         //new patron enters PQ (approaches window)  
                        PQArr[lineChoice] = listOfQs[lineChoice].Peek().Arrival;

                    }

                    Console.WriteLine("\t" + currentTime);
                    displayQs(listOfQs);

                }//end while for window departure




                if (PQ.Count >= listOfQs.Count)
                    filled = true;

                //This fills the PQ initially, otherwise they are 
                //pulled into the PQ when someone leaves.
                if (!filled)
                {
                    if (PQ2.Count < listOfQs.Count)                     //if the PQ (windows) are not full      
                    {
                        for (int i = 0; i < listOfQs.Count; i++)            //for every queue
                        {
                            if (listOfQs[i].Count > 0 && PQ2.Count < listOfQs.Count)    //if theres someone in the i'th Q and PQ2 is still not full
                            {
                                bool duplicate = false;
                                for (int j = 0; j < listOfQs.Count; j++)
                                {
                                    if (PQArr[j] != null && listOfQs[i].Peek().PatronNum == PQArr[j].Patron)
                                        duplicate = true;
                                }

                                if (!duplicate)
                                {
                                    listOfQs[i].Peek().Arrival = new Evnt(currentTime, i, listOfQs[i].Peek().PatronNum);     //and gets window wait time


                                    PQ2.Enqueue(listOfQs[i].Peek().Arrival);
                                    PQArr[i] = listOfQs[i].Peek().Arrival;
                                    //TODO test else, break here 

                                }
                            }
                        }
                    }
                }


                //Thread.Sleep();

                tickCounter++;
                currentTime = currentTime.Add(tick);
            }

            //summary at end
            double avgTime = totalTime.TotalSeconds / testLeaveWin;
            displayQs(listOfQs);
            string avgTimes = "";
            avgTimes += "The average service time for " + testLeaveWin + " Registrants was "
                      + avgTime + ".";

            Console.WriteLine(avgTimes);

            Console.WriteLine("fails: " + failDQCounter);
            Console.WriteLine("leave win:" + testLeaveWin);
            Console.WriteLine("Patrons: " + counterPatrons);
            totalTime = new TimeSpan();
            Console.WriteLine("Maximum Window Time: " + MaxWindowTime);
            Console.WriteLine("Minimum Window Time: " + MinWindowTime);

            Console.ReadLine();
        }

        private static int Poisson(double ExpectedValue)
        {
            double dLimit = -ExpectedValue;
            double dSum = Math.Log(ran.NextDouble());

            int Count;
            for (Count = 0; dSum > dLimit; Count++)
                dSum += Math.Log(ran.NextDouble());

            return Count;
        }

        private static double NegExp(double ExpectedValue, double minimum)
        {
            return (-(ExpectedValue - minimum) * Math.Log(ran.NextDouble(), Math.E) + minimum);
        }

        private static void RandomizeList(List<int> list)
        {
            for (int i = 0; i < list.Count * 5; i++)
                Swap(list, ran.Next(list.Count), ran.Next(list.Count));
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

        private static void Swap(List<int> list, int n, int m)
        {
            int temp = list[n];
            list[n] = list[m];
            list[m] = temp;
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

        #region Max Amount Of Time at The Window
        public static TimeSpan MinTimeAtWindow(TimeSpan min)
        {
            int results = TimeSpan.Compare(min, MinWindowTime);

            if (results > 0)
            {
                min = MinWindowTime;
            }
            return min;
        }
        #endregion

        #region Max Amount Of Time at The Window
        public static TimeSpan MaxTimeAtWindow(TimeSpan max)
        {
            int results = TimeSpan.Compare(max, MaxWindowTime);

            if(results < 0)
            {
                max = MaxWindowTime;
            }
            return max;
        }
        #endregion

        #region Displays The Queues 

        private static void displayQs(List<Queue<Registrants>> listOfQs)
        {

            List<List<int>> listToPrint = new List<List<int>>();
            List<int> intList;
            for (int i = 0; i < listOfQs.Count; i++)
            {
                Registrants[] tempArr = listOfQs[i].ToArray();

                intList = new List<int>(tempArr.Length);
                for (int j = 0; j < tempArr.Length; j++)
                {
                    //intList.Add(100);
                    intList.Add(tempArr[j].PatronNum);
                }
                listToPrint.Add(intList);
            }
            string listDisplay = "";

            listDisplay = $"\t\tRegistration Windows\n"
                        + "\t\t--------------------\n";

            for (int i = 0; i < listOfQs.Count; i++)
            {
                listDisplay += $"\t W {i}";
            }

            listDisplay += "\n";

            int max = 0;
            for (int i = 0; i < listOfQs.Count; i++)
            {
                if (listOfQs[i].Count > max)
                {
                    max = listOfQs[i].Count;
                }
            }

            for (int i = 0; i < max; i++)
            {
                for (int j = 0; j < listOfQs.Count; j++)
                {
                    if (listToPrint[j].Count > i)
                    {
                        listDisplay += "\t" + listToPrint[j][i];

                    }
                    else
                    {
                        listDisplay += "\t    ";
                    }
                }
                listDisplay += "\n";
            }
            listDisplay += "\n\n"
                         + "So far: -----------------------------------------------------------------\n"
                         + "Longest Queue Encountered So Far:\t" + longestQ + "\n"
                         + "Events Processed So Far:\t" + (testLeaveWin + counterPatrons) + "\tArrivals:\t" + counterPatrons + "\tDepartures:\t" + testLeaveWin;

            Console.Clear();
            Console.WriteLine(listDisplay);
        }

        #endregion

    }//end class
}//end namespace
