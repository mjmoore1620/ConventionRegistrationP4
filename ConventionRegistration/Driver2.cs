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

        private static TimeSpan totalTime = new TimeSpan();


        //[STAThread]
        static void Main(string[] args)
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
                        DoSimulation();
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
        }

        private static void DoSimulation()
        {
            //expectedRegistrationTime;
            //numberOfQs,
            //hoursOpen,
            //totalRegistrants;

            TimeSpan tick = new TimeSpan(100000000);                              //tick = .1 sec
            DateTime openTime = new DateTime(2016, 11, 1, 8, 0, 0, 0);
            TimeSpan hoursOpenTimeSpan = new TimeSpan(hoursOpen, 0, 0);
            DateTime closingTime = new DateTime(2016, 11, 1, 8, 0, 0, 0);
            closingTime += hoursOpenTimeSpan;


            
            int actualNumRegistrants = Poisson(totalExpectedRegistrants);       //actual number of registrants
            List<int> patronIdListFromPoisson = new List<int>(actualNumRegistrants);

            for (int i = 0; i < actualNumRegistrants; i++)
                patronIdListFromPoisson.Add(i);

            RandomizeList(patronIdListFromPoisson);



            TimeSpan enterConvetionTimer = new TimeSpan(hoursOpenTimeSpan.Ticks / actualNumRegistrants);     //how often people enter the convention
            double tickNumTrigger = enterConvetionTimer.Ticks / tick.Ticks;

            DateTime currentTime = openTime;

            List<Queue<Registrants>> listOfQs = new List<Queue<Registrants>>(numberOfQs);
            for (int i = 0; i < numberOfQs; i++)
                listOfQs.Add(new Queue<Registrants>());

            Queue<Registrants> expectedRegistrants = new Queue<Registrants>(actualNumRegistrants);

            for (int i = 0; i < actualNumRegistrants; i++)
                expectedRegistrants.Enqueue(new Registrants(patronIdListFromPoisson[i]));

            PriorityQueue<Registrants> PQ = new PriorityQueue<Registrants>();

            int testcounter = 0;
            testLeaveWin = 0;
            counterPatrons = 0;
            longestQ = 0;
            bool filled = false;
            int tickCounter = 0;
            //ListOfQueues print = new ListOfQueues(listOfQs);
            while (!(currentTime > closingTime && counterPatrons == testLeaveWin))//&& counterPatrons == testLeaveWin
            {
                if (tickCounter >= tickNumTrigger && expectedRegistrants.Count != 0)
                {
                    expectedRegistrants.Peek().lineChoice = ConventionRegistration.ShortestLine(listOfQs);
                    listOfQs[expectedRegistrants.Peek().lineChoice].Enqueue(expectedRegistrants.Dequeue());
                    tickCounter = 0;

                    if (longestQ < LongestLine(listOfQs))
                        longestQ = LongestLine(listOfQs); 

                    //Console.WriteLine(currentTime);
                    counterPatrons++;
                    
                }


                //if (PQ.Count < listOfQs.Count)                     //if the PQ (windows) are not full      
                //{
                //    for (int i = 0; i < listOfQs.Count; i++)            //for every queue
                //    {
                //        if (listOfQs[i].Count > 0)
                //        {
                //            listOfQs[i].Peek().Arrival = new Evnt(currentTime);     //and gets window wait time
                //            PQ.Enqueue(listOfQs[i].Peek());                    //First in line enters PQ

                //        }
                //    }
                //}

                while (PQ.Count > 0 && (PQ.Peek().Depart) <= currentTime)        //check the top of PQ
                {
                    
                    //Console.WriteLine("Patron Number: " + PQ.Peek().PatronNum);
                    //Console.WriteLine("Arrival Time: " + PQ.Peek().Arrival.Time);
                    //Console.WriteLine("Window Time: " + PQ.Peek().windowTime);
                    //Console.WriteLine("departtime: " + PQ.Peek().Depart);
                    //Console.WriteLine("Current Time" + currentTime);

                    int lineChoice = PQ.Peek().lineChoice;           //remember which queue the patron is leaving from


                    totalTime += PQ.Peek().windowTime;
                    PQ.Dequeue();                                          //the patron leaves the PQ (window)
                    testLeaveWin++;

                    try
                    {

                        listOfQs[lineChoice].Dequeue();                             //the patron leaves the queue (line they were waiting in)
                    }
                    catch (Exception)
                    {
                        //Console.WriteLine("failed");
                        testcounter++;
                    }

                    //if there is someone next in that line, they enter the PQ (approach the window) and are assigned a wait time
                    if (listOfQs[lineChoice].Count > 0)
                    {
                        listOfQs[lineChoice].Peek().Arrival = new Evnt(currentTime);            //assign wait time
                        listOfQs[lineChoice].Peek().SetDepart();
                        PQ.Enqueue(listOfQs[lineChoice].Peek());                         //new patron enters PQ (approaches window)   
                        Console.WriteLine("Patron Number: " + PQ.Peek().PatronNum);
                        Console.WriteLine("Arrival Time: " + PQ.Peek().Arrival.Time);
                        Console.WriteLine("Window Time: " + PQ.Peek().windowTime);
                        Console.WriteLine("departtime: " + PQ.Peek().Depart);
                        Console.WriteLine("Current Time" + currentTime);


                    }

                }

                if (PQ.Count >= listOfQs.Count)
                    filled = true;

                //This fills the PQ initially, otherwise they are 
                //pulled into the PQ when someone leaves.

                //if (!filled)
                //{
                if (PQ.Count < listOfQs.Count)                     //if the PQ (windows) are not full      
                {
                    for (int i = 0; i < listOfQs.Count; i++)            //for every queue
                    {
                        if (listOfQs[i].Count > 0)
                        {
                            if (PQ.Count < listOfQs.Count)
                            {
                                listOfQs[i].Peek().Arrival = new Evnt(currentTime);     //and gets window wait time
                                listOfQs[i].Peek().SetDepart();
                                PQ.Enqueue(listOfQs[i].Peek());                    //First in line enters PQ 
                            }

                        }
                    }
                }
                //}

                Console.Clear();
                Console.WriteLine("\t" + currentTime);
                displayQs(listOfQs);

                //Console.WriteLine("q1: " + listOfQs[0].Count);
                //Console.WriteLine("q2: " + listOfQs[1].Count);
                //Console.WriteLine("q3: " + listOfQs[2].Count);
                //Console.WriteLine("q4: " + listOfQs[3].Count);
                //Console.WriteLine("q5: " + listOfQs[4].Count);

                //string listDisplay = "";

                //listDisplay = $"\t\tRegistration Windows\n"
                //            + "\t\t" + currentTime + "\n"
                //            + "\t\t--------------------\n";

                //for (int i = 0; i < listOfQs.Count; i++)
                //{
                //    listDisplay += $"\t W {i}";
                //}

                //Console.WriteLine(listDisplay);
                //ListOfQueues print = new ListOfQueues(listOfQs);
                //Console.WriteLine(print.ToString());

                Thread.Sleep(5);
                


                tickCounter++;
                currentTime += tick;
            }
            double avgTime = totalTime.TotalSeconds / counterPatrons;


            displayQs(listOfQs);

            string avgTimes = "";

            avgTimes += "The average service time for " + counterPatrons + "Registrants was "
                      + avgTime + ".";



            Console.WriteLine("fails: " + testcounter);
            Console.WriteLine("leave win:" + testLeaveWin);

            //Console.WriteLine("q1: " + listOfQs[0].Count);
            //Console.WriteLine("q2: " + listOfQs[1].Count);
            //Console.WriteLine("q3: " + listOfQs[2].Count);
            //Console.WriteLine("q4: " + listOfQs[3].Count);
            //Console.WriteLine("q5: " + listOfQs[4].Count);

            //string listDisplay = "";

            //listDisplay = $"\t\tRegistration Windows\n"
            //            + "\t\t--------------------\n";

            //for (int i = 0; i < listOfQs.Count; i++)
            //{
            //    listDisplay += $"\t W {i}";
            //}

            //Console.WriteLine(listDisplay);
            //ListOfQueues print = new ListOfQueues(listOfQs);
            //Console.WriteLine(print.ToString());

            //displayQs(listOfQs);




            Console.WriteLine("Patrons: " + counterPatrons);

            Console.ReadLine();
        }

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
                    try
                    {
                        listDisplay += "\t" + listToPrint[j][i];
                    }
                    catch
                    {
                        listDisplay += "\t    ";
                    }
                }
                listDisplay += "\n";
            }
            listDisplay += "\n\n"
                         + "So far: -----------------------------------------------------------------\n"
                         + "Longest Queue Encountered So Far:\t" + longestQ + "\n"
                         + "Events Processed So Far:\t" + (testLeaveWin + counterPatrons) + "\tArrivals:\t" + counterPatrons + "\tDepartures:\t" + testLeaveWin ;

            Console.WriteLine(listDisplay);
        }

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

        private static double NegExp(double ExpectedValue, double minimum)
        {
            return (-(ExpectedValue - minimum) * Math.Log(ran.NextDouble(), Math.E) + minimum);
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

        private static void RandomizeList(List<int> list)
        {
            for (int i = 0; i < list.Count * 5; i++)
                Swap(list, ran.Next(list.Count), ran.Next(list.Count));
        }

        /// <summary>
        /// Swaps two places in a list
        /// </summary>
        /// <param name="list">The list containing the values to be swapped</param>
        /// <param name="n">first value to swap</param>
        /// <param name="m">second value to swap</param>
        private static void Swap(List<int> list, int n, int m)
        {
            int temp = list[n];
            list[n] = list[m];
            list[m] = temp;
        }



    }
}
