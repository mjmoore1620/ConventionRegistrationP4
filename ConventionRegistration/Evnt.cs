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
