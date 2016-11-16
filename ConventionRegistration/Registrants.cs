////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////
////	Project:		Project 4 - Convention Registration
////	File Name:		PriorityQueue.cs
////	Description:	PriorityQueue is a container class that prioritizes the items it contains based on those item's IComparable implementation. 
////	Course:			CSCI 2210-201 - Data Structures
////	Author:			Allison Ivey, iveyas@etsu.edu, Matthew Moore, zmjm40@etsu.edu, ETSU Graduate Students
////	Created:	    Nov 11, 2016
////	Copyright:		Allison Ivey, Matthew Moore, 2016
////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace ConventionRegistration
{
    class Registrants : IComparable
    {
       
        public int PatronNum { get; set; }
        public TimeSpan windowTime;
        
        public int lineChoice { get; set; }

        public Evnt Arrival { get; set; }

        private DateTime depart;

        public DateTime Depart
        {
            get { return depart; }
        }

        public void SetDepart()
        {
            depart = Arrival.Time.Add(windowTime);
        }

        private static Random ran = new Random();

        public Registrants(int patronNum)
        {
            PatronNum = patronNum;
            //windowTime = new TimeSpan(0, 0, 0, 0, (int)NegExp(270000.0, 90000.0));
        }



        //public int LineSize
        //{
        //    get { return LineSize; }
        //    set { LineSize = value; }
        //}

        //public void PopTheQueue(TimeSpan TimeAtWindow)
        //{
        //    DateTime Start = DateTime.Now;
        //    //Start - TimeAtWindow;
        //}

        public int CompareTo(object registrant)
        {
            Registrants e = (Registrants)registrant;
            return e.windowTime.CompareTo(windowTime);
        }

        private static double NegExp(double ExpectedValue, double minimum)
        {
            return (-(ExpectedValue - minimum) * Math.Log(ran.NextDouble(), Math.E) + minimum);
        }

        public override string ToString()
        {
            return PatronNum + ": " + Arrival.Depart;
        }

        
    }
}
