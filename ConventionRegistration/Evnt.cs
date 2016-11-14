using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public TimeSpan windowTime;

        private static Random ran = new Random();


        public Evnt(DateTime currentTime)
        {
            Type = EVENTTYPE.ENTER;
            Time = currentTime;
            Patron = -1;
            windowTime = new TimeSpan(0, 0, 0, 0, (int)NegExp(270000.0, 90000.0));
            //Duration = (int)NegExp(270000, 90000);
        }

        public Evnt (EVENTTYPE type, DateTime time, int patron)
        {
            Type = type;
            Time = time;
            Patron = patron;
        }

        public override string ToString()
        {
            //String str = "";
            //str += String.Format("Patron {0} ", Patron.ToString().PadLeft(3));
            //str += Type + "'s";
            //str += String.Format(" at {0}", Time.ToShortTimeString().PadLeft(8));
            

            return Time.ToString();
        }

        public int CompareTo(Object obj)
        {
            if (!(obj is Evnt))
                throw new ArgumentException("The argument is not an Event Object");

            Evnt e = (Evnt)obj;
            return (e.Time.CompareTo(Time));
        }

        private static double NegExp(double ExpectedValue, double minimum)
        {
            return (-(ExpectedValue - minimum) * Math.Log(ran.NextDouble(), Math.E) + minimum);
        }

        


    }
}
