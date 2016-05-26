using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses
{
    public class Helper
    {
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
