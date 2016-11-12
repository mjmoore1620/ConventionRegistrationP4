using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConventionRegistration
{
    class ConventionRegistration
    {

        public int ShortestLine(List<Queue<Registrants>> QueueList)
        {
            int ShortestLineFound=0;
            int min=1000;
            int temp;
            foreach(Queue<Registrants> c in QueueList)
            {
                temp = c.Count();

                if(temp<min)
                {
                    min = temp;
                }
            }

            return ShortestLineFound = min;
        }
       
    }

    
}
