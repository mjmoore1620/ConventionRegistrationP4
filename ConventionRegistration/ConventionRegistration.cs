using System;
using System.Collections.Generic;
using PriorityQueue_Wiener;
using System.Threading;

namespace ConventionRegistration
{
    /// <summary>
    /// Does all of the simulation and registration of the people
    /// </summary>
    class ConventionRegistration
    {
        /// <summary>
        /// The random object
        /// </summary>
        private static Random ran = new Random();           //uniform random number generator
        /// <summary>
        /// The total time
        /// </summary>
        private static TimeSpan totalTime,                  //The total windowTime
                                                            /// <summary>
                                                            /// The maximum window time
                                                            /// </summary>
                                MaxWindowTime,              //max amount of time at the window
                                                            /// <summary>
                                                            /// The minimum window time
                                                            /// </summary>
                                MinWindowTime;              //min amount of time at the window

        /// <summary>
        /// Gets the test when to leave window.
        /// </summary>
        /// <value>
        /// The test when to leave window.
        /// </value>
        public static int testLeaveWin { get; private set; }
        /// <summary>
        /// Gets the counter patrons.
        /// </summary>
        /// <value>
        /// The counter patrons.
        /// </value>
        public static int counterPatrons { get; private set; }
        /// <summary>
        /// Gets the longest queue.
        /// </summary>
        /// <value>
        /// The longest queue.
        /// </value>
        public static int longestQ { get; private set; }


        /// <summary>
        /// Does the simulation.
        /// </summary>
        /// <param name="ExpectedRegistrants">The expected registrants.</param>
        /// <param name="NumberOfHoursOpen">The number of hours open.</param>
        /// <param name="NumberOfQueues">The number of queues.</param>
        /// <param name="ExpectedRegistrationTime">The expected registration time.</param>
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
                    expectedRegistrants.Peek().lineChoice = GetShortestLine(listOfQs);
                    listOfQs[expectedRegistrants.Peek().lineChoice].Enqueue(expectedRegistrants.Dequeue());

                    if (longestQ < LongestLine(listOfQs))       //save the length of longest Queue
                        longestQ = LongestLine(listOfQs);

                    displayQs(listOfQs);

                    counterPatrons++;
                }//end while to find the shortest line


                while (PQ2.Count > 0 && PQ2.Peek().Depart <= currentTime)
                {


                    int lineChoice = PQ2.Peek().LineChoice;
                    MaxWindowTime= MaxTimeAtWindow(PQ2.Peek().windowTime);
                    MinWindowTime = MinTimeAtWindow(PQ2.Peek().windowTime);

                    totalTime += PQ2.Peek().windowTime;
                  
                    PQ2.Dequeue();  //get out of the priority queue
                    testLeaveWin++; //tests when to leave the window and queue


                    
                    listOfQs[lineChoice].Dequeue();                             //the patron leaves the queue (line they were waiting in)
                    PQArr[lineChoice] = null;
                  

                    //if there is someone next in that line, they enter the PQ (approach the window) and are assigned a wait time
                    if (listOfQs[lineChoice].Count > 0)
                    {
                        listOfQs[lineChoice].Peek().Arrival = new Evnt(currentTime, lineChoice, listOfQs[lineChoice].Peek().PatronNum, ExpectedRegistrationTime);            //assign wait time

                        PQ2.Enqueue(listOfQs[lineChoice].Peek().Arrival);                         //new patron enters PQ (approaches window)  
                        PQArr[lineChoice] = listOfQs[lineChoice].Peek().Arrival;

                    }//end if of patron entering

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
                                    listOfQs[i].Peek().Arrival = new Evnt(currentTime, i, listOfQs[i].Peek().PatronNum, ExpectedRegistrationTime);     //and gets window wait time


                                    PQ2.Enqueue(listOfQs[i].Peek().Arrival);
                                    PQArr[i] = listOfQs[i].Peek().Arrival;
                                    //TODO test else, break here 

                                }
                            }
                        }
                    }
                }


                

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

        /// <summary>
        /// Does the simulation without display.
        /// </summary>
        /// <param name="ExpectedRegistrants">The expected registrants.</param>
        /// <param name="NumberOfHoursOpen">The number of hours open.</param>
        /// <param name="NumberOfQueues">The number of queues.</param>
        /// <param name="ExpectedRegistrationTime">The expected registration time.</param>
        static void DoSimulationWithoutDisplay(int ExpectedRegistrants, int NumberOfHoursOpen, int NumberOfQueues, double ExpectedRegistrationTime)
        {

            TimeSpan tick = new TimeSpan(1000000);                              //tick = .1 sec
            DateTime openTime = new DateTime(2016, 11, 1, 8, 0, 0, 0);
            TimeSpan hoursOpenTimeSpan = new TimeSpan(NumberOfHoursOpen, 0, 0);
            DateTime closingTime = new DateTime(2016, 11, 1, 8, 0, 0, 0);
            closingTime += hoursOpenTimeSpan;

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
            while (!((currentTime > closingTime) && counterPatrons == testLeaveWin))
            {

                while (counterPatrons != actualNumRegistrants && entranceTimesInTicks[counterPatrons] == tickCounter)
                {
                    expectedRegistrants.Peek().lineChoice = GetShortestLine(listOfQs);
                    listOfQs[expectedRegistrants.Peek().lineChoice].Enqueue(expectedRegistrants.Dequeue());

                    if (longestQ < LongestLine(listOfQs))       //save the length of longest Queue
                        longestQ = LongestLine(listOfQs);

                    //displayQs(listOfQs);

                    counterPatrons++;
                }

                while (PQ2.Count > 0 && PQ2.Peek().Depart <= currentTime)           //check if the top of PQ should depart
                {

                    int lineChoice = PQ2.Peek().LineChoice;                     //remember which queue the patron is leaving from

                    totalTime += PQ2.Peek().windowTime;
                    PQ2.Dequeue();                                              //the patron leaves the PQ (window)
                    testLeaveWin++;

                    listOfQs[lineChoice].Dequeue();                             //the patron leaves the queue (line they were waiting in)
                    PQArr[lineChoice] = null;


                    //if there is someone next in that line, they enter the PQ (approach the window) and are assigned a wait time
                    if (listOfQs[lineChoice].Count > 0)
                    {
                        listOfQs[lineChoice].Peek().Arrival = new Evnt(currentTime, lineChoice, listOfQs[lineChoice].Peek().PatronNum, ExpectedRegistrationTime);            //assign wait time

                        PQ2.Enqueue(listOfQs[lineChoice].Peek().Arrival);                         //new patron enters PQ (approaches window)  
                        PQArr[lineChoice] = listOfQs[lineChoice].Peek().Arrival;

                    }

                    //Console.WriteLine("\t" + currentTime);
                    //displayQs(listOfQs);

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
                                    listOfQs[i].Peek().Arrival = new Evnt(currentTime, i, listOfQs[i].Peek().PatronNum, ExpectedRegistrationTime);     //and gets window wait time

                                    PQ2.Enqueue(listOfQs[i].Peek().Arrival);         //First in line enters PQ 
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

        }

        /// <summary>
        /// Does the simulation x times.
        /// </summary>
        /// <param name="ExpectedRegistrants">The expected registrants.</param>
        /// <param name="NumberOfHoursOpen">The number of hours open.</param>
        /// <param name="NumberOfQueues">The number of queues.</param>
        /// <param name="ExpectedRegistrationTime">The expected registration time.</param>
        /// <param name="numberOfSimulations">The number of simulations.</param>
        public static void DoSimulationXTimes(int ExpectedRegistrants, int NumberOfHoursOpen, int NumberOfQueues, double ExpectedRegistrationTime, int numberOfSimulations)
        {
            double maxQLengthSum = 0;
            int highestMaxQueueLength = 0;
            int lowestMaxQueueLength = int.MaxValue;

            for (int i = 0; i < numberOfSimulations; i++)
            {
                DoSimulationWithoutDisplay(ExpectedRegistrants, NumberOfHoursOpen, NumberOfQueues,  ExpectedRegistrationTime);
                maxQLengthSum += longestQ;
                if (highestMaxQueueLength < longestQ)
                    highestMaxQueueLength = longestQ;
                if (lowestMaxQueueLength > longestQ)
                    lowestMaxQueueLength = longestQ;
            }
            //prints the longest Queue length in X number of simulations
            Console.WriteLine($"Highest max queue length of {numberOfSimulations} number of simulations: {highestMaxQueueLength}");
            Console.WriteLine($"Lowest max queue length of {numberOfSimulations} number of simulations: {lowestMaxQueueLength}");

        }

        /// <summary>
        /// Poissons the specified expected value.
        /// </summary>
        /// <param name="ExpectedValue">The expected value.</param>
        /// <returns></returns>
        private static int Poisson(double ExpectedValue)
        {
            double dLimit = -ExpectedValue;
            double dSum = Math.Log(ran.NextDouble());

            int Count;
            for (Count = 0; dSum > dLimit; Count++)
                dSum += Math.Log(ran.NextDouble());

            return Count;
        }

        /// <summary>
        /// Randomizes the list.
        /// </summary>
        /// <param name="list">The list.</param>
        private static void RandomizeList(List<int> list)
        {
            for (int i = 0; i < list.Count * 5; i++)
                Swap(list, ran.Next(list.Count), ran.Next(list.Count));
        }

        /// <summary>
        /// Gets the shortest line.
        /// </summary>
        /// <param name="listOfQs">The list of qs.</param>
        /// <returns></returns>
        public static int GetShortestLine(List<Queue<Registrants>> listOfQs)
        {
            int smallestCount = 10000;                  
            int indexOfQueue = -1;

            for (int i = 0; i < listOfQs.Count; i++)
            {
                if (listOfQs[i].Count < smallestCount)
                {
                    smallestCount = listOfQs[i].Count;      //save shorter Queue length 
                    indexOfQueue = i;                       //save index of shorter line
                }
            }

            return indexOfQueue;
        }

        /// <summary>
        /// Swaps the specified list.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="n">The n.</param>
        /// <param name="m">The m.</param>
        private static void Swap(List<int> list, int n, int m)
        {
            int temp = list[n];
            list[n] = list[m];
            list[m] = temp;
        }

        /// <summary>
        /// Longests the line.
        /// </summary>
        /// <param name="listOfQs">The list of qs.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Minimums the time at window.
        /// </summary>
        /// <param name="min">The minimum.</param>
        /// <returns></returns>
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
        /// <summary>
        /// Maximums the time at window.
        /// </summary>
        /// <param name="max">The maximum.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Displays the queues.
        /// </summary>
        /// <param name="listOfQs">The list of queues.</param>
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
            Thread.Sleep(5);

        }

        #endregion

    }//end class
}//end namespace
