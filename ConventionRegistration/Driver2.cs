//////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
//	Project:		Project 4 - Convention Registration
//	File Name:		Driver.cs
//	Description:	Driver for Project 4 - Convention Registration. All output is handled here
//	Course:			CSCI 2210-201 - Data Structures
//	Author:			Allison Ivey, iveyas@etsu.edu, Matthew Moore, zmjm40@etsu.edu, ETSU Graduate Students
//	Created:	    Nov 11, 2016
//	Copyright:		Allison Ivey, Matthew Moore, 2016
//
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using PriorityQueue_Wiener;
using System;
using System.Collections.Generic;
using static ConventionRegistration.ConventionRegistration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConventionRegistration
{
    class Driver
    {
        private static Random ran = new Random();
        //private static Queue<Registrants> line1 = new Queue<Registrants>();
        //private static Queue<Registrants> line2 = new Queue<Registrants>();
        //private static Queue<Registrants> line3 = new Queue<Registrants>();
        //private static Queue<Registrants> line4 = new Queue<Registrants>();
        //private static Queue<Registrants> line5 = new Queue<Registrants>();
        
        
        //private static DateTime currentTime;
        //private static DateTime closingTime;
        
        //private static Registrants[] nextInLine;
        //private static List<List<int>> listToPrint = new List<List<int>>();
        //private static int registrantTotal;


        private static double expectedRegistrationTime = 4.5;             //Default values
        private static int numberOfQs = 5;                                //Default values
        private static int hoursOpen = 10;                                //Default values
        private static int totalExpectedRegistrants = 1000;                       //Default values
        private static int longestQ;
        private static int testLeaveWin;
        private static int counterPatrons;
        //ConventionRegistration conventionRegistration = new ConventionRegistration();

        private static TimeSpan totalTime = new TimeSpan();


        //[STAThread]
        static void Main(string[] args)
        {
            //must be true to exit menu
            bool exitMenu = false;

            #region Main Menu Loop
            //loops main menu
            do
            {
                //prints menu options to user
                Console.Write(MenuString());
                //user menuchoice input
                string MenuChoice = Console.ReadLine();

                //main menu structure
                switch (MenuChoice)
                {
                    case "1":
                        Console.Clear();
                        setRegistrantTotal();
                        Console.Clear();
                        break;

                    case "2":
                        Console.Clear();
                        setConventionDuration();
                        Console.Clear();
                        break;

                    case "3":
                        Console.Clear();
                        setNumberOfQs();
                        Console.Clear();
                        break;
                    case "4":
                        Console.Clear();
                        setExpectedRegistrationTime();
                        Console.Clear();
                        break;

                    case "5":
                        DoSimulation(totalExpectedRegistrants, hoursOpen, numberOfQs, expectedRegistrationTime);
                        break;

                    case "6":
                        exitMenu = true;
                        Console.WriteLine("Goodbye");
                        Console.ReadLine();
                        break;

                    default:
                        Console.Clear();
                        Console.Write("\n Enter a number from 1 to 6. \n");
                        Console.Clear();
                        break;
                }
                //exit loop if exitMenu = true 
               
            } while (!exitMenu);
            #endregion
        }

        //private static void DoSimulation()
        //{

        //    TimeSpan tick = new TimeSpan(1000000);                              //tick = .1 sec
        //    DateTime openTime = new DateTime(2016, 11, 1, 8, 0, 0, 0);
        //    TimeSpan hoursOpenTimeSpan = new TimeSpan(hoursOpen, 0, 0);
        //    DateTime closingTime = new DateTime(2016, 11, 1, 8, 0, 0, 0);
        //    closingTime += hoursOpenTimeSpan;

        //    int actualNumRegistrants = Poisson(totalExpectedRegistrants);       //actual number of registrants
        //    List<int> patronIdListFromPoisson = new List<int>(actualNumRegistrants);

        //    for (int i = 0; i < actualNumRegistrants; i++)
        //        patronIdListFromPoisson.Add(i);

        //    RandomizeList(patronIdListFromPoisson);

        //    TimeSpan enterConvetionTimer = new TimeSpan(hoursOpenTimeSpan.Ticks / actualNumRegistrants);     //how often people enter the convention
        //    double tickNumTrigger = enterConvetionTimer.Ticks / tick.Ticks;

        //    int numberOfTotalTicks = (int)(hoursOpenTimeSpan.Ticks / tick.Ticks);     //how often people enter the convention

        //    List<int> entranceTimesInTicks = new List<int>(actualNumRegistrants);
        //    for (int i = 0; i < actualNumRegistrants; i++)
        //        entranceTimesInTicks.Add(ran.Next(numberOfTotalTicks));

        //    entranceTimesInTicks.Sort();

        //    DateTime currentTime = openTime;

        //    List<Queue<Registrants>> listOfQs = new List<Queue<Registrants>>(numberOfQs);
        //    for (int i = 0; i < numberOfQs; i++)
        //        listOfQs.Add(new Queue<Registrants>());

        //    Queue<Registrants> expectedRegistrants = new Queue<Registrants>(actualNumRegistrants);

        //    for (int i = 0; i < actualNumRegistrants; i++)
        //        expectedRegistrants.Enqueue(new Registrants(patronIdListFromPoisson[i]));

        //    PriorityQueue<Registrants> PQ = new PriorityQueue<Registrants>();
        //    PriorityQueue<Evnt> PQ2 = new PriorityQueue<Evnt>();
        //    Evnt[] PQArr = new Evnt[numberOfQs];

        //    int failDQCounter = 0;
        //    testLeaveWin = 0;
        //    counterPatrons = 0;
        //    longestQ = 0;
        //    bool filled = false;
        //    int tickCounter = 0;

        //    while (!((currentTime > closingTime ) && counterPatrons == testLeaveWin))//&& counterPatrons == testLeaveWin
        //    {

        //        while (counterPatrons != actualNumRegistrants && entranceTimesInTicks[counterPatrons] == tickCounter)
        //        {
        //            expectedRegistrants.Peek().lineChoice = ConventionRegistration.ShortestLine(listOfQs);
        //            listOfQs[expectedRegistrants.Peek().lineChoice].Enqueue(expectedRegistrants.Dequeue());

        //            if (longestQ < LongestLine(listOfQs))       //save the length of longest Queue
        //                longestQ = LongestLine(listOfQs);

        //            counterPatrons++;
        //        }


        //        while (PQ2.Count > 0 && PQ2.Peek().Depart <= currentTime)
        //        {


        //            int lineChoice = PQ2.Peek().LineChoice;


        //            totalTime += PQ2.Peek().windowTime;
        //            PQ2.Dequeue();
        //            testLeaveWin++;

        //            try
        //            {
        //                listOfQs[lineChoice].Dequeue();                             //the patron leaves the queue (line they were waiting in)
        //                PQArr[lineChoice] = null;
        //            }
        //            catch (Exception)
        //            {
        //                failDQCounter++;
        //            }

        //            //if there is someone next in that line, they enter the PQ (approach the window) and are assigned a wait time
        //            if (listOfQs[lineChoice].Count > 0)
        //            {
        //                listOfQs[lineChoice].Peek().Arrival = new Evnt(currentTime, lineChoice, listOfQs[lineChoice].Peek().PatronNum);            //assign wait time

        //                PQ2.Enqueue(listOfQs[lineChoice].Peek().Arrival);                         //new patron enters PQ (approaches window)  
        //                PQArr[lineChoice] = listOfQs[lineChoice].Peek().Arrival;

        //            }

        //            Console.WriteLine("\t" + currentTime);
        //            displayQs(listOfQs);

        //        }//end while for window departure




        //        if (PQ.Count >= listOfQs.Count)
        //            filled = true;

        //        //This fills the PQ initially, otherwise they are 
        //        //pulled into the PQ when someone leaves.
        //        if (!filled)
        //        {
        //            if (PQ2.Count < listOfQs.Count)                     //if the PQ (windows) are not full      
        //            {
        //                for (int i = 0; i < listOfQs.Count; i++)            //for every queue
        //                {
        //                    if (listOfQs[i].Count > 0 && PQ2.Count < listOfQs.Count)    //if theres someone in the i'th Q and PQ2 is still not full
        //                    {
        //                        bool duplicate = false;
        //                        for (int j = 0; j < listOfQs.Count; j++)
        //                        {
        //                            if (PQArr[j] != null && listOfQs[i].Peek().PatronNum == PQArr[j].Patron)
        //                                duplicate = true;
        //                        }

        //                        if (!duplicate)
        //                        {
        //                            listOfQs[i].Peek().Arrival = new Evnt(currentTime, i, listOfQs[i].Peek().PatronNum);     //and gets window wait time


        //                            PQ2.Enqueue(listOfQs[i].Peek().Arrival);
        //                            PQArr[i] = listOfQs[i].Peek().Arrival;
        //                            //TODO test else, break here 
        //                        }
        //                    }
        //                }
        //            }
        //        }


        //        //Thread.Sleep();

        //        tickCounter++;
        //        currentTime = currentTime.Add(tick);
        //    }

        //    //summary at end
        //    double avgTime = totalTime.TotalSeconds / testLeaveWin;
        //    displayQs(listOfQs);
        //    string avgTimes = "";
        //    avgTimes += "The average service time for " + testLeaveWin + " Registrants was "
        //              + avgTime + ".";

        //    Console.WriteLine(avgTimes);

        //    Console.WriteLine("fails: " + failDQCounter);
        //    Console.WriteLine("leave win:" + testLeaveWin);
        //    Console.WriteLine("Patrons: " + counterPatrons);

        //    Console.ReadLine();
        //}



        //private static void displayQs(List<Queue<Registrants>> listOfQs)
        //{

        //    List<List<int>> listToPrint = new List<List<int>>();
        //    List<int> intList;
        //    for (int i = 0; i < listOfQs.Count; i++)
        //    {
        //        Registrants[] tempArr = listOfQs[i].ToArray();

        //        intList = new List<int>(tempArr.Length);
        //        for (int j = 0; j < tempArr.Length; j++)
        //        {
        //            //intList.Add(100);
        //            intList.Add(tempArr[j].PatronNum);
        //        }
        //        listToPrint.Add(intList);
        //    }
        //    string listDisplay = "";

        //    listDisplay = $"\t\tRegistration Windows\n"
        //                + "\t\t--------------------\n";

        //    for (int i = 0; i < listOfQs.Count; i++)
        //    {
        //        listDisplay += $"\t W {i}";
        //    }

        //    listDisplay += "\n";

        //    int max = 0;
        //    for (int i = 0; i < listOfQs.Count; i++)
        //    {
        //        if (listOfQs[i].Count > max)
        //        {
        //            max = listOfQs[i].Count;
        //        }
        //    }

        //    for (int i = 0; i < max; i++)
        //    {
        //        for (int j = 0; j < listOfQs.Count; j++)
        //        {
        //            if (listToPrint[j].Count > i)
        //            {
        //                listDisplay += "\t" + listToPrint[j][i];

        //            }
        //            else
        //            {
        //                listDisplay += "\t    ";
        //            }
        //        }
        //        listDisplay += "\n";
        //    }
        //    listDisplay += "\n\n"
        //                 + "So far: -----------------------------------------------------------------\n"
        //                 + "Longest Queue Encountered So Far:\t" + longestQ + "\n"
        //                 + "Events Processed So Far:\t" + (testLeaveWin + counterPatrons) + "\tArrivals:\t" + counterPatrons + "\tDepartures:\t" + testLeaveWin;

        //    Console.Clear();
        //    Console.WriteLine(listDisplay);
        //}

        //#endregion

       
        #region Set Expected Registration Time
        private static void setExpectedRegistrationTime()
        {
            Console.Write("  What is the expected service time for a Registrant in minutes? \n"
                          + "  Example: Enter 5.5 for 5 and half minutes (5 minutes, 30 seconds).");
            string userInput = Console.ReadLine();
            if (double.TryParse(userInput, out expectedRegistrationTime))
                Console.WriteLine("Expected registration time = " + expectedRegistrationTime);
            else
                Console.WriteLine("  Invalid expected registration time entered. ");
            Console.ReadLine();
        } 
        #endregion

        #region Set Number Of Queues 
        private static void setNumberOfQs()
        {
            Console.Write("  How many registration lines are to be simulated?: ");
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out numberOfQs))
                Console.WriteLine("Number of lines = " + numberOfQs);
            else
                Console.WriteLine("  Invalid number of window lines entered. ");
            Console.ReadLine();
        } 
        #endregion

        #region Set Convention Duration
        private static void setConventionDuration()
        {
            Console.Write("  How many hours will registration be open?: ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out hoursOpen))
                Console.WriteLine("Number of hours of operation = " + hoursOpen);
            else
                Console.WriteLine("  Invalid number of hours of operation entered. ");
            Console.ReadLine();
        } 
        #endregion

        #region Set Registrant Total 
        private static void setRegistrantTotal()
        {
            Console.Write("  How many registrants are expected to be served in a day?: ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out totalExpectedRegistrants))
                Console.WriteLine("Expected total registrants = " + totalExpectedRegistrants);
            else
                Console.WriteLine("  Invalid number of expected Registrants entered. ");
            Console.ReadLine();
        } 
        #endregion

        #region Menu String
        private static string MenuString()
        {
            string menuString = "";

            menuString = "\t  Simulation Menu\n"
                       + "\t  ---------------\n"
                       + "\t1. Set the number of Registrants\n"
                       + "\t2. Set the number of hours of operation\n"
                       + "\t3. Set the number of windows\n"
                       + "\t4. Set the expected checkout duration\n"
                       + "\t5. Run the simulation\n"
                       + "\t6. End the program\n\n"
                       + "\t  Type the number of your choice from the menu: ";

            return menuString;
        } 
        #endregion

        #region Main Menu Switch Statement 
        private static void MainMenu()
        {
            //must be true to exit menu
            bool exitMenu = false;

            //loops main menu
            do
            {
                //prints menu options to user
                Console.Write(MenuString());
                //user menuchoice input
                string MenuChoice = Console.ReadLine();

                //main menu structure
                switch (MenuChoice)
                {
                    case "1":
                        Console.Clear();
                        Console.Write("  How many registrants are expected to be served in a day?: ");
                        Console.ReadLine();
                        //setRegistrantTotal(Console.ReadLine());
                        Console.Clear();
                        break;

                    case "2":
                        Console.Clear();
                        Console.Write("  How many hours will registration be open?: ");
                        Console.ReadLine();
                        //setConventionDuration(Console.ReadLine());
                        Console.Clear();
                        break;

                    case "3":
                        Console.Clear();
                        Console.Write("  How many registration lines are to be simulated?: ");
                        Console.ReadLine();
                        //setLineCount(Console.ReadLine());
                        Console.Clear();
                        break;
                    case "4":
                        Console.Clear();
                        Console.Write("  What is the expected service time for a Registrant in minutes? \n"
                                         + "  Example: Enter 5.5 for 5 and half minutes (5 minutes, 30 seconds).");
                        Console.ReadLine();
                        //setExpectedRegistrationTime(Console.ReadLine());
                        Console.Clear();
                        break;

                    case "5":
                        Console.Clear();
                        Console.ReadLine();
                        //run the simulation
                        break;

                    case "6":
                        exitMenu = true;
                        break;

                    default:
                        Console.Clear();
                        Console.Write("\n Enter a number from 1 to 6. \n");
                        Console.Clear();
                        break;
                }
                //exit loop if exitMenu = true
            } while (!exitMenu);
        }
        #endregion

        #region distribution and util
        //private static double NegExp(double ExpectedValue, double minimum)
        //{
        //    return (-(ExpectedValue - minimum) * Math.Log(ran.NextDouble(), Math.E) + minimum);
        //}

        //private static int Poisson(double ExpectedValue)
        //{
        //    double dLimit = -ExpectedValue;
        //    double dSum = Math.Log(ran.NextDouble());

        //    int Count;
        //    for (Count = 0; dSum > dLimit; Count++)
        //        dSum += Math.Log(ran.NextDouble());

        //    return Count;
        //}

        //private static void RandomizeList(List<int> list)
        //{
        //    for (int i = 0; i < list.Count * 5; i++)
        //        Swap(list, ran.Next(list.Count), ran.Next(list.Count));
        //}

        /// <summary>
        /// Swaps two places in a list
        /// </summary>
        /// <param name="list">The list containing the values to be swapped</param>
        /// <param name="n">first value to swap</param>
        /// <param name="m">second value to swap</param>
        //private static void Swap(List<int> list, int n, int m)
        //{
        //    int temp = list[n];
        //    list[n] = list[m];
        //    list[m] = temp;
        //}

        #endregion


    }
}
