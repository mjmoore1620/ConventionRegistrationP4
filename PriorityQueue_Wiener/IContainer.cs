using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue_Wiener
{
    interface IContainer<T>
    {
        //removes all objects from container
        void Clear();
        //returns true if container is empty
        bool IsEmpty();
        //returns the number of entries in the container
        int Count { get; set; }


    }
}
