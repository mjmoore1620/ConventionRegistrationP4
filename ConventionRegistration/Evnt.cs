﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////
////	Project:		Project 4 - Convention Registration
////	File Name:		Evnt.cs
////	Description:	 
////	Course:			CSCI 2210-201 - Data Structures
////	Author:			Allison Ivey, iveyas@etsu.edu, Matthew Moore, zmjm40@etsu.edu, ETSU Graduate Students
////	Created:	    Nov 11, 2016
////	Copyright:		Allison Ivey, Matthew Moore, 2016
////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;

namespace ConventionRegistration
{
    /// <summary>
    /// 
    /// </summary>
    enum EVENTTYPE {  ENTER, LEAVE }
    class Evnt : IComparable
    {
        public EVENTTYPE Type { get; set; }
        public DateTime Time { get; set; }
        public int Patron { get; set; }
        public double Duration
        {
            get { return Duration; }
            set { Duration = value; }
        }

        public int LineChoice { get; private set; }

        public TimeSpan windowTime;

        private static Random ran = new Random();

        private DateTime depart;

        public DateTime Depart
        {
            get { return depart; }
        }


        public void SetDepart()
        {
            depart = Time.Add(windowTime);
        }


        public Evnt(DateTime currentTime, int lineChoice, int patronNum)
        {
            Type = EVENTTYPE.ENTER;
            Time = currentTime;
            windowTime = new TimeSpan(0, 0, 0, 0, (int)NegExp(270000.0, 90000.0));
            LineChoice = lineChoice;
            Patron = patronNum;
            SetDepart();

        }

        //public Evnt (EVENTTYPE type, DateTime time, int patron)
        //{
        //    Type = type;
        //    Time = time;
        //    Patron = patron;
        //}

        public override string ToString()
        {
            //String str = "";
            //str += String.Format("Patron {0} ", Patron.ToString().PadLeft(3));
            //str += Type + "'s";
            //str += String.Format(" at {0}", Time.ToShortTimeString().PadLeft(8));
            

            return Time.ToString();
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that 
        /// indicates whether the current instance precedes, follows, or occurs in the same position in the 
        /// sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has 
        /// these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in 
        /// the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. 
        /// Greater than zero This instance follows <paramref name="obj" /> in the sort order.
        /// </returns>
        /// <exception cref="System.ArgumentException">The argument is not an Event Object</exception>
        public int CompareTo(Object obj)
        {
            if (!(obj is Evnt))
                throw new ArgumentException("The argument is not an Event Object");

            Evnt e = (Evnt)obj;
            return (e.depart.CompareTo(Depart));
        }

        private static double NegExp(double ExpectedValue, double minimum)
        {
            return (-(ExpectedValue - minimum) * Math.Log(ran.NextDouble(), Math.E) + minimum);
        }

        


    }
}
