////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////
////	Project:		Project 4 - Convention Registration
////	File Name:		PriorityQueue.cs
////	Description:	PriorityQueue is a container class that prioritizes the items it contains based on those item's IComparable implementation. 
////	Course:			CSCI 2210-201 - Data Structures
////	Author:			Allison Ivey, iveyas@etsu.edu, Matthew Moore, zmjm40@etsu.edu, ETSU Graduate Students
////	Created:	    Nov 11, 2016
////	Copyright:		Allison Ivey, Matthew Moore, 2016
////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//commenting complete

using System;

namespace PriorityQueue_Wiener
{
    /// <summary>
    /// PriorityQueue is a container class that prioritizes the items it contains based on those item's IComparable implementation. 
    /// </summary>
    /// <typeparam name="T">Type of object the PriorityQueue contains.</typeparam>
    /// <seealso cref="PriorityQueue_Wiener.IPriorityQueue{T}" />
    public class PriorityQueue<T> : IPriorityQueue<T> where T: IComparable
    {
        //fields and properties
        private Node top;               //reference to the top of the PQ

        public int Count { get; set; }
     
        /// <summary>
        /// Enqueues the specified item and inserts the item based on it's IComparable value
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
        /// <returns>The top item</returns>
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
                
                //returns variable, still garbage collects oldNode,
                //this is different from slides, because why not return the item?
                var item = oldNode.Item;
                oldNode = null;                 
                return item;
                
                
                //oldNode = null;             //so oldNode will be garbage collected
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            top = null;
        }
            
        /// <summary>
        /// Peeks at the top instance.
        /// </summary>
        /// <returns>Returns the top item in the PQ.</returns>
        /// <exception cref="System.InvalidOperationException">Cannot obtain top of priority queue.</exception>
        public T Peek()
        {
            if (!IsEmpty())
                return top.Item;
            else
                throw new InvalidOperationException("Cannot obtain top of priority queue.");
        }
 
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
            public T Item { get; set; }                 //the actual object referred to by the node
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
