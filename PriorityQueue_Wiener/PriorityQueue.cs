using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue_Wiener
{
    public class PriorityQueue<T> : IPriorityQueue<T> where T: IComparable
    {
        //fields and properties
        private Node top;               //reference to the top of the PQ

        public int Count { get; set; }

        //Add an item to the PQ
        public void Enqueue(T item)
        {
            if (Count == 0)
                top = new Node(item, null);
            else
            {
                Node current = top;
                Node previous = null;

                //Search for the first node in the linked structure that is smaller than the item
                while (current != null && current.Item.CompareTo(item) >= 0)
                {
                    previous = current;
                    current = current.Next;
                }

                //Have found the place to insert the new node
                Node newNode = new Node(item, current);

                //If there is a previous node, set it to the new node
                if (previous != null)
                    previous.Next = newNode;
                else
                    top = newNode;
            }
            Count++;        // add 1 to the number of nodes in the PQ
        }




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


}
