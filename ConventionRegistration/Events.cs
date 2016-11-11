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
    enum EVENTTYPE { ENTER, LEAVE }
    class Evnts : IComparable
    {
        public EVENTTYPE Type { get; set; }
        public DateTime Time { get; set; }
        public int Patron { get; set; }

        public Evnts()
        {
            Type = EVENTTYPE.ENTER;
            Time = DateTime.Now;
            Patron = -1;
        }

        public Evnts(EVENTTYPE type, DateTime time, int patron)
        {
            Type = type;
            Time = time;
            Patron = patron;
        }

        public override string ToString()
        {
            String str = "";
            str += String.Format("Patron {0} ", Patron.ToString().PadLeft(3));
            str += Type + "'s";
            str += String.Format(" at {0}", Time.ToShortTimeString().PadLeft(8));

            return str;
        }

        public int CompareTo(Object obj)
        {
            if (!(obj is Evnts))
                throw new ArgumentException("The argument is not an Event Object");

            Evnts e = (Evnts)obj;
            return (e.Time.CompareTo(Time));
        }



    }
}
