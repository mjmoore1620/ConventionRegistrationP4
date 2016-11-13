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
        private static Queue<Registrants> line1 = new Queue<Registrants>();
        private static Queue<Registrants> line2 = new Queue<Registrants>();
        private static List<Queue<Registrants>> listOfQs = new List<Queue<Registrants>>();
        private static PriorityQueue<Registrants> windows = new PriorityQueue<Registrants>();
        private static List<int> patronIdListFromPoisson;
        private static DateTime currentTime;
        private static DateTime closingTime;
        private static DateTime openTime;
        private static Registrants[] nextInLine;
        private static List<List<int>> listToPrint= new List<List<int>>();

        [STAThread]
        static void Main(string[] args)
        {
            listOfQs.Add(line1);
            listOfQs.Add(line2);

            nextInLine = new Registrants[(listOfQs.Count)];
            double time = 360000;                               //in ticks or 1/10 of a sec 
            int expected = 20;//Poisson(1000);      //TODO test
            patronIdListFromPoisson = new List<int>(expected);
            for (int i = 0; i < expected; i++)
            {
                patronIdListFromPoisson.Add(i);
            }
            RandomizeList(patronIdListFromPoisson);
            //foreach (var item in fishNum)
            //{
            //    Console.WriteLine(item);
            //}

            double enqueueRate = time / expected;

            openTime = new DateTime(2016, 11, 1, 7, 0, 0, 0);
            closingTime = new DateTime(2016, 11, 1, 17, 0, 0, 0);
            TimeSpan timeOpen = (closingTime - openTime);

            //TimeSpan tick = new TimeSpan(1000000);                  //tick = .1 sec
            TimeSpan tick = new TimeSpan(1000000 * 1000);               //tick = 100 sec

            currentTime = openTime;                                 //set initial current time 
            int patronEntranceThreshold = 0;                        //increases until this >= the enqueue rate
            int patronCounter = 0;                                  //used for index of the randomized patronNumber list
            
            //continue simulation until 
            while (currentTime < closingTime)
            {
                
                if (enqueueRate <= patronEntranceThreshold)
                {
                    //patron comes in, they choose the shortest line
                    Registrants newGuy = new Registrants(patronIdListFromPoisson[patronCounter]);
                    newGuy.lineChoice = ConventionRegistration.ShortestLine(listOfQs);
                    listOfQs[ConventionRegistration.ShortestLine(listOfQs)].Enqueue(newGuy);

                    patronCounter++;                  //enqueue the next person next time
                    patronEntranceThreshold = 0;
                }

                int lineChoice = -1;
                if (windows.Count > 0)
                {
                    //Console.WriteLine(windows.Peek().Arrival.Time + windows.Peek().windowTime);
                    //Console.WriteLine(currentTime);

                    //while there are patrons in the PQ and they have waited (since they arrived at window + windowTime) until currentTime is <=
                    //(while) instead of (if) incase multiple patron's wait time is up in same tick
                    while (windows.Count > 0 && (windows.Peek().Arrival.Time + windows.Peek().Arrival.windowTime) <= currentTime)
                    {
                        
                        lineChoice = windows.Peek().lineChoice;                     //remember which queue the patron is leaving from     
                        
                                
                        //Console.WriteLine(windows.Dequeue().ToString());
                        Console.WriteLine(windows.Peek().ToString());
                        foreach (var item in listOfQs[lineChoice].ToArray())
                        {
                            //if()
                            //Console.WriteLine(item.PatronNum);
                        }

                        //this is the only way patrons are dequeued
                        windows.Dequeue();                                          //the patron leaves the PQ (window)
                        listOfQs[lineChoice].Dequeue();                             //the patron leaves the queue (line they were waiting in)

                        //if there is someone next in that line, they enter the PQ (approach the window) and are assigned a wait time
                        if (listOfQs[lineChoice].Count > 0)
                        {
                            windows.Enqueue(listOfQs[lineChoice].Peek());                           //new patron enters PQ (approaches window)       
                            listOfQs[lineChoice].Peek().Arrival = new Evnt(currentTime);            //assign wait time
                        }
                        //Console.WriteLine(107);
                    }
                }

                //set nextInLine array
                //this is currently unused
                for (int i = 0; i < listOfQs.Count; i++)
                {
                    if (listOfQs[i].Count > 0)
                        nextInLine[i] = listOfQs[i].Peek();
                }

                //This fills the PQ initially, otherwise they are 
                //pulled into the PQ when someone leaves.
                if (windows.Count < listOfQs.Count)                     //if the PQ (windows) are not full      
                {
                    for (int i = 0; i < listOfQs.Count; i++)            //for every queue
                    {
                        if (listOfQs[i].Count > 0)                      
                        {
                            windows.Enqueue(listOfQs[i].Peek());                    //First in line enters PQ
                            listOfQs[i].Peek().Arrival = new Evnt(currentTime);     //and gets window wait time
                        }
                    }
                }

                currentTime += tick;
                patronEntranceThreshold++;

                //for (int i = 0; i < listOfQs.Count; i++)
                //{
                //    Registrants[] tempArr = listOfQs[i].ToArray();

                //    List<int> intList = new List<int>(tempArr.Length);
                //    for (int j = 0; j < tempArr.Length; j++)
                //    {
                //        //intList.Add(100);
                //        intList.Add(tempArr[j].PatronNum);
                //    }
                //    listToPrint.Add(intList);
                //}
                //string listDisplay = "";

                //listDisplay = $"\t\tRegistration Windows\n"
                //            + "\t\t--------------------\n";

                //for(int i = 0; i < listOfQs.Count; i++)
                //{
                //    listDisplay += $"\t W {i}";
                //}

                //listDisplay += "\n";
                //int max = 0;
                //for (int i = 0; i < listOfQs.Count; i++)
                //{
                //    if (listOfQs[i].Count> max)
                //    {
                //        max = listOfQs[i].Count;
                //    }
                //}

                //for (int i = 0; i < max; i++)
                //{
                //    for (int j = 0; j < listOfQs.Count; j++)
                //    {
                //        try
                //        { 
                //            listDisplay += "\t" + listToPrint[j][i];
                //        }
                //        catch
                //        {
                //            listDisplay+= "\t    ";
                //        }
                //    }
                //}

                //Console.WriteLine(156);
                //Thread.Sleep(20);
                //    Console.WriteLine("its time");
<<<<<<< HEAD
                
=======


                ListOfQueues print = new ListOfQueues(listOfQs);
                Console.WriteLine(print.ToString());
>>>>>>> refs/remotes/origin/Allison4.0
                //Console.WriteLine(listDisplay);

            }//end while
            
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
                        Console.Write ("  What is the expected service time for a Registrant in minutes? \n"
                                         + "  Example: Enter 5.5 for 5 and half minutes (5 minutes, 30 seconds)." );
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
