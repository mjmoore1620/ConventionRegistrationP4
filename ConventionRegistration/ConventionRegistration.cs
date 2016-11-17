using System;
using System.Collections.Generic;
using PriorityQueue_Wiener;
using System.Threading;
using System.Globalization;

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


        private static int longestQSum;


        #region Does The Simulation 
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

            PriorityQueue<Evnt> PQ2 = new PriorityQueue<Evnt>();
            Evnt[] PQArr = new Evnt[NumberOfQueues];
            
            testLeaveWin = 0;
            counterPatrons = 0;
            longestQ = 0;
            int tickCounter = 0;

            while (!((currentTime > closingTime) && counterPatrons == testLeaveWin))//&& counterPatrons == testLeaveWin
            {

                while (counterPatrons != actualNumRegistrants && entranceTimesInTicks[counterPatrons] == tickCounter)
                {
                    GetShortestLine(listOfQs, expectedRegistrants);
                }//end while to find the shortest line


                while (PQ2.Count > 0 && PQ2.Peek().Depart <= currentTime)
                {
                    int lineChoice = GoToPriorityQueue(listOfQs, PQ2, PQArr);

                    //if there is someone next in that line, they enter the PQ (approach the window) and are assigned a wait time
                    if (listOfQs[lineChoice].Count > 0)
                    {
                        listOfQs[lineChoice].Peek().Arrival = new Evnt(currentTime, lineChoice, listOfQs[lineChoice].Peek().PatronNum, ExpectedRegistrationTime);            //assign wait time

                        PQ2.Enqueue(listOfQs[lineChoice].Peek().Arrival);                         //new patron enters PQ (approaches window)  
                        PQArr[lineChoice] = listOfQs[lineChoice].Peek().Arrival;

                    }//end if of patron entering
                    
                    displayQs(listOfQs);

                }//end while for window departure

                

                //This fills the PQ initially, otherwise they are 
                //pulled into the PQ when someone leaves.
                    EnterPriorityQueue(ExpectedRegistrationTime, currentTime, listOfQs, PQ2, PQArr);




                tickCounter++;
                currentTime = currentTime.Add(tick);
            }

            //summary at end
            double avgTime = (totalTime.TotalSeconds / testLeaveWin)/60;
            displayQs(listOfQs);
            string avgTimes = "";
            avgTimes += "The average service time for " + testLeaveWin + " Registrants was "
                      + avgTime.ToString("0.##") + ".";

            Console.WriteLine(avgTimes);
            totalTime = new TimeSpan();
            Console.WriteLine(("Maximum Window Time: ") + ("{0:%h} hours {0:%m} minutes {0:%s} seconds"), MaxWindowTime);
            Console.WriteLine(("Minimum Window Time: ") + ("{0:%h} hours {0:%m} minutes {0:%s} seconds"), MinWindowTime);
            
        } 
        #endregion

        #region Exit The Priority Queue
        /// <summary>
        /// Registrant enters the priority queue.
        /// </summary>
        /// <param name="ExpectedRegistrationTime">The expected registration time.</param>
        /// <param name="currentTime">The current time.</param>
        /// <param name="listOfQs">The list of qs.</param>
        /// <param name="PQ2">The p q2.</param>
        /// <param name="PQArr">The pq arr.</param>
        private static void EnterPriorityQueue(double ExpectedRegistrationTime, DateTime currentTime, List<Queue<Registrants>> listOfQs, PriorityQueue<Evnt> PQ2, Evnt[] PQArr)
        {
            if (PQ2.Count < listOfQs.Count)                     //if the PQ (windows) are not full      
            {
                for (int i = 0; i < listOfQs.Count; i++)            //for every queue
                {
                    if (listOfQs[i].Count > 0 && PQ2.Count < listOfQs.Count)    //if theres someone in the i'th Q and PQ2 is still not full
                    {
                        //make sure Registrants don't go to a window that isn't there own
                        bool duplicate = false;
                        for (int j = 0; j < listOfQs.Count; j++)
                        {
                            if (PQArr[j] != null && listOfQs[i].Peek().PatronNum == PQArr[j].Patron)
                                duplicate = true;
                        }

                        if (!duplicate)
                        {
                            listOfQs[i].Peek().Arrival = new Evnt(currentTime, i, listOfQs[i].Peek().PatronNum, ExpectedRegistrationTime);     //get window wait time
                            
                            PQ2.Enqueue(listOfQs[i].Peek().Arrival);
                            PQArr[i] = listOfQs[i].Peek().Arrival;

                        }
                    }
                }
            }
        } 
        #endregion

        #region Goes To The Priority Queue
        /// <summary>
        /// Goes to priority queue.
        /// </summary>
        /// <param name="listOfQs">The list of qs.</param>
        /// <param name="PQ2">The p q2.</param>
        /// <param name="PQArr">The pq arr.</param>
        /// <returns></returns>
        private static int GoToPriorityQueue(List<Queue<Registrants>> listOfQs, PriorityQueue<Evnt> PQ2, Evnt[] PQArr)
        {
            int lineChoice = PQ2.Peek().LineChoice;
            MaxWindowTime = MaxTimeAtWindow(PQ2.Peek().windowTime);
            MinWindowTime = MinTimeAtWindow(PQ2.Peek().windowTime);

            totalTime += PQ2.Peek().windowTime;

            PQ2.Dequeue();  //get out of the priority queue
            testLeaveWin++; //tests when to leave the window and queue



            listOfQs[lineChoice].Dequeue();                             //the patron leaves the queue (line they were waiting in)
            PQArr[lineChoice] = null;
            return lineChoice;
        } 
        #endregion

        #region Gets The Shortest Line
        /// <summary>
        /// Gets the shortest line.
        /// </summary>
        /// <param name="listOfQs">The list of qs.</param>
        /// <param name="expectedRegistrants">The expected registrants.</param>
        private static void GetShortestLine(List<Queue<Registrants>> listOfQs, Queue<Registrants> expectedRegistrants)
        {
            expectedRegistrants.Peek().lineChoice = GetShortestLine(listOfQs);
            listOfQs[expectedRegistrants.Peek().lineChoice].Enqueue(expectedRegistrants.Dequeue());

            if (longestQ < LongestLine(listOfQs))       //save the length of longest Queue
                longestQ = LongestLine(listOfQs);

            displayQs(listOfQs);

            counterPatrons++;
        } 
        #endregion

        #region Used For Testing Window Time And Length Based On Window Number With No Visual Element
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
            
            PriorityQueue<Evnt> PQ2 = new PriorityQueue<Evnt>();
            Evnt[] PQArr = new Evnt[NumberOfQueues];

            int failDQCounter = 0;
            testLeaveWin = 0;
            counterPatrons = 0;
            longestQ = 0;
            

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
                    

                }//end while for window departure




                
                //This fills the PQ initially, otherwise they are 
                //pulled into the PQ when someone leaves.
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
            totalTime = new TimeSpan();

            Console.WriteLine("fails: " + failDQCounter);
            Console.WriteLine("leave win:" + testLeaveWin);
            Console.WriteLine("Patrons: " + counterPatrons);

        } 
        #endregion

        #region Does The Simulation x Times
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
            longestQSum = 0;


            List<List<int>> MaxQRecord = new List<List<int>>();
            List<int[]> qs = new List<int[]>();
            int[,] qqs = new int[100, 1];

            //if (!qs[1][i.Contains(longestQ))
            //    qs.Add(longestQ);


            //for (int i = 0; i < qs.Count; i++)
            //{
            //    if (qs[i]. != longestQ)
            //    {

            //    }
            //}
            




            for (int i = 0; i < numberOfSimulations; i++)
            {
                DoSimulationWithoutDisplay(ExpectedRegistrants, NumberOfHoursOpen, NumberOfQueues, ExpectedRegistrationTime);
                maxQLengthSum += longestQ;
                if (highestMaxQueueLength < longestQ)
                    highestMaxQueueLength = longestQ;
                if (lowestMaxQueueLength > longestQ)
                    lowestMaxQueueLength = longestQ;

                
                longestQSum += longestQ;
            }

            double avgMaxQ = longestQSum / numberOfSimulations;
            //prints the longest Queue length in X number of simulations
            Console.WriteLine($"Highest max queue length of {numberOfSimulations} number of simulations: {highestMaxQueueLength}");
            Console.WriteLine($"Lowest max queue length of {numberOfSimulations} number of simulations: {lowestMaxQueueLength}");
            Console.WriteLine($"Mean max queue length of {numberOfSimulations} number of simulations: {avgMaxQ}");

        } 
        #endregion

        #region Poisson specifies the expected Value
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
        #endregion

        #region Randomizes The List of Registrants
        /// <summary>
        /// Randomizes the list.
        /// </summary>
        /// <param name="list">The list.</param>
        private static void RandomizeList(List<int> list)
        {
            for (int i = 0; i < list.Count * 5; i++)
                Swap(list, ran.Next(list.Count), ran.Next(list.Count));
        } 
        #endregion

        #region Gets The Shortest Line
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
        #endregion

        #region Swap the specified indexes
        /// <summary>
        /// Swaps the specified indexes.
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
        #endregion

        #region Longest Line
        /// <summary>
        /// Longest line.
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
        #endregion

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
            Thread.Sleep(10);

        }

        #endregion

    }//end class
}//end namespace
