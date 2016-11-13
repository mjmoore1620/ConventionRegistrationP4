using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConventionRegistration
{
    class ListOfQueues
    {
        List<Queue<Registrants>> ListOfRegistrentsQueues = new List<Queue<Registrants>>();
        public ListOfQueues(List<Queue<Registrants>> ListOfRegistrentsQueues)
        {
            this.ListOfRegistrentsQueues = ListOfRegistrentsQueues;
        }

        public override String ToString()
        {
            String printString= "";
            Boolean hasNext= true;
            Queue<Registrants>[] arrayOfQueues = ListOfRegistrentsQueues.ToArray();
            while (hasNext == true)
            {
                for (int i = 0; i < arrayOfQueues.Length; i++)
                {
                    if (i != arrayOfQueues.Length - 1)
                    {
                        hasNext = false;
                        try
                        {
                            printString += arrayOfQueues[i].Peek().ToString() + "\t";
                            arrayOfQueues[i].Dequeue();
                            hasNext = true;
                        }
                        catch
                        {
                            printString += "    \t";
                        }
                    }
                    else
                    {
                        try
                        {
                            printString += arrayOfQueues[i].Peek().ToString() + "\n";
                            arrayOfQueues[i].Dequeue();
                            hasNext = true;
                        }
                        catch
                        {
                            printString += "    \n";
                        }

                    }

                } 
            }

            return printString;
        }
    }
}
