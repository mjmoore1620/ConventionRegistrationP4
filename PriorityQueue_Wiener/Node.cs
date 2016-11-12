using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue_Wiener
{
    private class Node
    {
        //properties
        public T Item { get; set; }
        public Node Next { get; set; }

        //constructor
        public Node(T value, Node link)
        {
            Item = value;
            Next = link;
        }
    }
}
