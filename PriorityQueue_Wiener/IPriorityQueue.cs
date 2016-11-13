using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue_Wiener
{
    interface IPriorityQueue<T> : IContainer<T> where T : IComparable
    {
        //inserts item based on its priority
        void Enqueue(T item);

        //removes the item in the queue
        T Dequeue();

        //Query
        T Peek();


    }
}
