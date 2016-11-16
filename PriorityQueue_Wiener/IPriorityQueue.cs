////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////
////	Project:		Project 4 - Convention Registration
////	File Name:		IPriorityQueue.cs
////	Description:	IPriorityQueue interface that provides funtionality for the PriorityQueue container class. 
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
    /// IPriorityQueue interface that provides funtionality for the PriorityQueue container class. 
    /// </summary>
    /// <typeparam name="T">The type that the PriorityQueue container holds. T must implement IComparable.</typeparam>
    /// <seealso cref="PriorityQueue_Wiener.IContainer{T}" />
    interface IPriorityQueue<T> : IContainer<T> where T : IComparable
    {
        //inserts item based on its priority
        void Enqueue(T item);

        //removes the item in the queue and returns that item
        T Dequeue();

        //Query
        T Peek();
    }
}
