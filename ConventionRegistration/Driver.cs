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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConventionRegistration
{
    class Driver
    {
        private static Random ran = new Random();
        private static Queue<Registrants> line = new Queue<Registrants>();
        private static List<int> patrons;

        static void Main(string[] args)
        {
            //Console.WriteLine(MenuString());
            //timeInLine();

            ////this is a to test the average of NegExp()
            //List<double> nums = new List<double>(1000);
            //for (int i = 0; i < 9999; i++)
            //{
            //    nums.Add(Poisson(300));
            //    //nums.Add(NegExp(3000));
            //}

            //double sum = 0;
            //foreach (var num in nums)
            //{
            //    sum += num;
            //}

            //Console.WriteLine(sum / 9999.0);

            //MainMenu();

            double time = 36000000;
            int expected = Poisson(1000);
            patrons = new List<int>(expected);
            for (int i = 0; i < expected; i++)
            {
                patrons.Add(i);
            }
            RandomizeList(patrons);

            double rate = time / expected;

            for (int i = 0; i < expected; i++)
                line.Enqueue(new Registrants(patrons[i]));

            while (line.Count > 0)
                Console.WriteLine(line.Dequeue().PatronNum);

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
        
        private static double NegExp(double ExpectedValue)
        {
            return (-ExpectedValue * Math.Log(ran.NextDouble(), Math.E) + 1500.0);
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
            {
                Swap(list, ran.Next(list.Count), ran.Next(list.Count));
            }

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
