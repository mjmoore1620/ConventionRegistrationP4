using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriorityQueue_Wiener
{
    //this is a change
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="PriorityQueue_Wiener.IPriorityQueue{T}" />
    public class PriorityQueue<T> : IPriorityQueue<T> where T: IComparable
    {
        //fields and properties
        /// <summary>
        /// The top
        /// </summary>
        private Node top;               //reference to the top of the PQ

        /// <summary>
        /// Gets or sets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count { get; set; }

        //Add an item to the PQ
        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
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

        //Remove an item from the priority queue and discard it
        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Cannot remove from empty queue.</exception>
        public T Dequeue()
        {
            if (IsEmpty())
                throw new InvalidOperationException("Cannot remove from empty queue.");
            else
            {
                Node oldNode = top;
                top = top.Next;
                Count--;
                
                //returns variable, still garbage collects oldNode
                var item = oldNode.Item;
                oldNode = null;
                return item;
                
                //We changed this
                //oldNode = null;             //so oldNode will be garbage collected
            }
        }

        //Make PQ empty
        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            top = null;
        }

        //Retrieve the top item on the PQ
        /// <summary>
        /// Peeks this instance.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Cannot obtain top of priority queue.</exception>
        public T Peek()
        {
            if (!IsEmpty())
                return top.Item;
            else
                throw new InvalidOperationException("Cannot obtain top of priority queue.");
        }

        //asks whether the PQ is empty
        /// <summary>
        /// Determines whether this instance is empty.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmpty()
        { 

            return Count == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <seealso cref="PriorityQueue_Wiener.IPriorityQueue{T}" />
        private class Node
        {
            //properties
            /// <summary>
            /// Gets or sets the item.
            /// </summary>
            /// <value>
            /// The item.
            /// </value>
            public T Item { get; set; }
            /// <summary>
            /// Gets or sets the next.
            /// </summary>
            /// <value>
            /// The next.
            /// </value>
            public Node Next { get; set; }

            //constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="value">The value.</param>
            /// <param name="link">The link.</param>
            public Node(T value, Node link)
            {
                Item = value;
                Next = link;
            }
        }
    }


}
