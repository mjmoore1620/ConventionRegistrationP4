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

        static void Main(string[] args)
        {
            //Console.WriteLine(MenuString());
            //timeInLine();

            //this is a to test the average of NegExp()
            List<double> nums = new List<double>(1000);
            for (int i = 0; i < 9999; i++)
            {
                nums.Add(Poisson(300));
                //nums.Add(NegExp(3000));
            }

            double sum = 0;
            foreach (var num in nums)
            {
                sum += num;
            }

            Console.WriteLine(sum / 9999.0);

            

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
                       + "\t  Type the number of your choice from the menu:";
            
            return menuString;
        }

        private static void MainMenu()
        {
            bool menuExit = false;
        }


        //private static void timeInLine()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        Console.WriteLine(NegExp(3000));
        //    }
        //}
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


        
        
    }
}
