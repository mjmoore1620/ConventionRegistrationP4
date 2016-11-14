using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConventionRegistration
{
    class Registrants : IComparable
    {
        public Evnt Arrival;
        public int PatronNum { get; set; }
        public TimeSpan windowTime;
        
        public int lineChoice { get; set; }

        private DateTime depart;

        public DateTime Depart
        {
            get { return depart; }
        }

        public void SetDepart()
        {
            depart = Arrival.Time + windowTime;
        }

        private static Random ran = new Random();

        public Registrants(int patronNum)
        {
            PatronNum = patronNum;
            windowTime = new TimeSpan(0, 0, 0, 0, (int)NegExp(270000.0, 90000.0));
        }

        

        public int LineSize
        {
            get { return LineSize; }
            set { LineSize = value; }
        }

        public void PopTheQueue(TimeSpan TimeAtWindow)
        {
            DateTime Start = DateTime.Now;
            //Start - TimeAtWindow;
        }

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
            return PatronNum + ": " + windowTime.ToString();
        }

        
    }
}
