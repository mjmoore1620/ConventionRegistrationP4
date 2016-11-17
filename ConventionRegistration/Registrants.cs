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
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.IComparable" />
    class Registrants : IComparable
    {

        /// <summary>
        /// Gets or sets the patron number.
        /// </summary>
        /// <value>
        /// The patron number.
        /// </value>
        public int PatronNum { get; set; }
        /// <summary>
        /// The window time
        /// </summary>
        public TimeSpan windowTime;

        /// <summary>
        /// Gets or sets the line choice.
        /// </summary>
        /// <value>
        /// The line choice.
        /// </value>
        public int lineChoice { get; set; }

        /// <summary>
        /// Gets or sets the arrival.
        /// </summary>
        /// <value>
        /// The arrival.
        /// </value>
        public Evnt Arrival { get; set; }

        /// <summary>
        /// The depart
        /// </summary>
        private DateTime depart;

        /// <summary>
        /// Gets the depart.
        /// </summary>
        /// <value>
        /// The depart.
        /// </value>
        public DateTime Depart
        {
            get { return depart; }
        }

        /// <summary>
        /// Sets the depart.
        /// </summary>
        public void SetDepart()
        {
            depart = Arrival.Time.Add(windowTime);
        }

        /// <summary>
        /// The ran
        /// </summary>
        private static Random ran = new Random();

        /// <summary>
        /// Initializes a new instance of the <see cref="Registrants"/> class.
        /// </summary>
        /// <param name="patronNum">The patron number.</param>
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

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="registrant">The registrant.</param>
        /// <returns></returns>
        public int CompareTo(object registrant)
        {
            Registrants e = (Registrants)registrant;
            return e.windowTime.CompareTo(windowTime);
        }

        /// <summary>
        /// Negs the exp.
        /// </summary>
        /// <param name="ExpectedValue">The expected value.</param>
        /// <param name="minimum">The minimum.</param>
        /// <returns></returns>
        private static double NegExp(double ExpectedValue, double minimum)
        {
            return (-(ExpectedValue - minimum) * Math.Log(ran.NextDouble(), Math.E) + minimum);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return PatronNum + ": " + Arrival.Depart;
        }

        
    }
}
