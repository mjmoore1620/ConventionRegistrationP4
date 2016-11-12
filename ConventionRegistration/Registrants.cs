using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConventionRegistration
{
    class Registrants
    {
        Evnts Arrival = new Evnts();
        public Registrants(int PatronNum)
        {

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

    }
}
