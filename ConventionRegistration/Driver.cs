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
using static ConventionRegistration.ConventionRegistration;

namespace ConventionRegistration
{
    /// <summary>
    /// The driver of the program
    /// </summary>
    class Driver
    {
        /// <summary>
        /// The random object
        /// </summary>
        private static Random ran = new Random();

        /// <summary>
        /// The expected registration time
        /// </summary>
        private static double expectedRegistrationTime = 4.5;                       //Default values
        /// <summary>
        /// The number of queues
        /// </summary>
        private static int numberOfQs = 9;                                          //Default values
        /// <summary>
        /// The hours open
        /// </summary>
        private static int hoursOpen = 10;                                          //Default values
        /// <summary>
        /// The total expected registrants
        /// </summary>
        private static int totalExpectedRegistrants = 1000;                         //Default values
        /// <summary>
        /// The number of simulations
        /// </summary>
        private static int numberOfSimulations = 10;

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [STAThread] 
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
                        //Console.Clear();
                        break;

                    case "2":
                        Console.Clear();
                        setConventionDuration();
                        //Console.Clear();
                        break;

                    case "3":
                        Console.Clear();
                        setNumberOfQs();
                        Console.Clear();
                        break;
                    case "4":
                        Console.Clear();
                        setExpectedRegistrationTime();
                        //Console.Clear();
                        break;

                    case "5":
                        Console.Clear();
                        DoSimulation(totalExpectedRegistrants, hoursOpen, numberOfQs, expectedRegistrationTime);
                        EnterToContinue();
                        break;

                    case "6":
                        Console.Clear();
                        if(SetNumberOfSimulationRuns())
                        {
                            DoSimulationXTimes(totalExpectedRegistrants, hoursOpen, numberOfQs, expectedRegistrationTime, numberOfSimulations);
                            EnterToContinue();
                        }
                        break;

                    case "7":
                        exitMenu = true;
                        Console.WriteLine("Goodbye");
                        EnterToContinue();
                        break;


                    default:
                        Console.Clear();
                        Console.Write("\n Enter a number from 1 to 6. \n");
                        EnterToContinue();
                        //Console.Clear();
                        break;
                }
                //exit loop if exitMenu = true 

                Console.Clear();
            } while (!exitMenu);
            #endregion
        }


        /// <summary>
        /// Sets the number of simulation runs.
        /// </summary>
        private static bool SetNumberOfSimulationRuns()
        {
            Console.Write("  How many times do you want to run the simulation with the current settings? ");
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out numberOfSimulations))
            {
                Console.WriteLine("Number of simulations: " + numberOfSimulations);
                EnterToContinue();
                return true;
            }
            else
            {
                Console.WriteLine("  Invalid number of simulations entered. ");
                EnterToContinue();
                return false;
            }

        }

        /// <summary>
        /// Sets the expected registration time.
        /// </summary>
        private static void setExpectedRegistrationTime()
        {
            Console.Write("  What is the expected service time for a Registrant in minutes? \n"
                          + "  Example: Enter 5.5 for 5 and half minutes (5 minutes, 30 seconds).");
            string userInput = Console.ReadLine();
            if (double.TryParse(userInput, out expectedRegistrationTime))
                Console.WriteLine("Expected registration time = " + expectedRegistrationTime);
            else
                Console.WriteLine("  Invalid expected registration time entered. ");
            EnterToContinue();
        }

        #region Set Number Of Queues 
        /// <summary>
        /// Sets the number of qs.
        /// </summary>
        private static void setNumberOfQs()
        {
            Console.Write("  How many registration lines are to be simulated?: ");
            string userInput = Console.ReadLine();
            if (int.TryParse(userInput, out numberOfQs))
                Console.WriteLine("Number of lines = " + numberOfQs);
            else
                Console.WriteLine("  Invalid number of window lines entered. ");
            EnterToContinue();
        }
        #endregion

        #region Set Convention Duration
        /// <summary>
        /// Sets the duration of the convention.
        /// </summary>
        private static void setConventionDuration()
        {
            Console.Write("  How many hours will registration be open?: ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out hoursOpen))
                Console.WriteLine("Number of hours of operation = " + hoursOpen);
            else
                Console.WriteLine("  Invalid number of hours of operation entered. ");
            EnterToContinue();
        }
        #endregion

        #region Set Registrant Total 
        /// <summary>
        /// Sets the registrant total.
        /// </summary>
        private static void setRegistrantTotal()
        {
            Console.Write("  How many registrants are expected to be served in a day?: ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out totalExpectedRegistrants))
                Console.WriteLine("Expected total registrants = " + totalExpectedRegistrants);
            else
                Console.WriteLine("  Invalid number of expected Registrants entered. ");
            EnterToContinue();
        }
        #endregion

        #region Menu String
        /// <summary>
        /// Menus the string.
        /// </summary>
        /// <returns></returns>
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
                       + "\t6. Run the simulation multiple times\n"
                       + "\t7. End the program\n\n"
                       + "\t  Type the number of your choice from the menu: ";

            return menuString;
        }
        #endregion

        /// <summary>
        /// Press Enter to continue during runtime.
        /// </summary>
        public static void EnterToContinue()
        {
            Console.Write("Press enter to continue...");
            Console.ReadLine();
        }
        
        

    }//end class
}//end namespace
