
using System;
using UnityEngine;

namespace ImportantClasses
{
    /// <summary>
    /// this class only adds functionality to existing classes
    /// </summary>
    public static class ClassExtender
    {
        /// <summary>
        /// gets an orthogonal Vector of the original Vector
        /// </summary>
        /// <param name="v">the original Vector</param>
        /// <returns></returns>
        public static Vector2 Normal(this Vector2 v)
        {
            Vector2 normal;
            if (v.y.Equals(0f))
                normal = new Vector2(0, 1);
            else if (v.x.Equals(0f))
                normal = new Vector2(1, 0);
            else
                normal = new Vector2(1, v.x / v.y);

            return normal;
        }

        /// <summary>
        /// calculates the intersection point between two straights of the type g(x)= p+ x*v
        /// </summary>
        /// <param name="v1">the direction vector of the first straight</param>
        /// <param name="v2">the direction vector of the second straight</param>
        /// <param name="p2">the starting point of the second straight</param>
        /// <param name="p1">the starting point of the first straight. By default its (0|0)</param>
        /// <returns>the intersection point of the two straights</returns>
        public static Vector2 IntersectionPoint(this Vector2 v1, Vector2 v2,
            Vector2 p2, Vector2 p1 = default(Vector2))
        {
            //p1 + v1 * k = p2 + v2 * j
            //rearragne equation to j and calculate it
            float j = ((p1.x - p2.x)/v2.x + (v1.x/v2.x)*((p2.y - p1.y)/v1.y))/
                      (1 - (v1.x*v2.y/(v2.x*v1.y)));
            //intersection point = p2 + v2 * j
            return new Vector2(p2.x + v2.x*j, p2.y + v2.y*j);
        }

        /// <summary>
        /// calculates the angle between two vectors
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float GetAngle(this Vector2 v1, Vector2 v2)
        {
            return (float) Math.Acos((v1.x*v2.x + v1.y + v2.y)/v1.magnitude*v2.magnitude);
        }
    }
}
