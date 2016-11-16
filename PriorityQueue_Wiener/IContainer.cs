////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////
////	Project:		Project 4 - Convention Registration
////	File Name:		IContainer.cs
////	Description:	IContainer interface that provides funtionality for the PriorityQueue container class. 
////	Course:			CSCI 2210-201 - Data Structures
////	Author:			Allison Ivey, iveyas@etsu.edu, Matthew Moore, zmjm40@etsu.edu, ETSU Graduate Students
////	Created:	    Nov 11, 2016
////	Copyright:		Allison Ivey, Matthew Moore, 2016
////
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//commenting complete

namespace PriorityQueue_Wiener
{
    /// <summary>
    /// IContainer interface provides functionality for the PriorityQueue container class. 
    /// </summary>
    /// <typeparam name="T">The type of objects the container holds</typeparam>
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
