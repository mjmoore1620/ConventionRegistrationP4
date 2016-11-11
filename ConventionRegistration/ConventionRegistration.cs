using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConventionRegistration
{
    class ConventionRegistration
    {

        private TimeSpan TimeAtRegistration;
        private DateTime ArrivalTime;
        private DateTime DepartureTime;
        private TimeSpan durationTime; 
        public DateTime Arrival
        {
            get {return ArrivalTime; }
            set {ArrivalTime = value; }

        }
        
        public DateTime Departure
        {
            get {return DepartureTime; }
            set {DepartureTime = value; }
        }
        public TimeSpan duration
        {
            get {return durationTime; }
            set {durationTime = value; }
        }
       

        public TimeSpan MyProperty
        {
            get { return TimeAtRegistration; }
            set { TimeAtRegistration = value; }
        }


    }

    
}
