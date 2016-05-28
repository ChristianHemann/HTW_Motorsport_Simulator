using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses
{
    /// <summary>
    /// provides functions which are often needed
    /// </summary>
    public class Helper
    {
        /// <summary>
        /// Checks if the content of the arrays is Equal.
        /// The normal Equal function of an array just returns true if both arrays are the exactly same object.
        /// </summary>
        /// <typeparam name="T">Type of both arrays</typeparam>
        /// <param name="arr1">The first array to compare</param>
        /// <param name="arr2">The second array which will be compared to the first one</param>
        /// <returns>true when the arrays have the same content; false then not</returns>
        public static bool ArrayValueEqual<T>(T[] arr1, T[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;
            for (int i = 0; i < arr1.Length; i++)
            {
                if(!arr1[i].Equals(arr2[i]))
                    return false;
            }
            return true;
        }
    }
}
