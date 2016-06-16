
using System;
using MathNet.Numerics.LinearAlgebra;

namespace ImportantClasses
{
    /// <summary>
    /// this class only adds functionality to existing classes
    /// </summary>
    public static class ClassExtender
    {
        /// <summary>
        /// gets an orthogonal Vector of the original 2-dimensional Vector which direction shows to the right of the given Vector
        /// </summary>
        /// <param name="v">the original Vector</param>
        /// <returns></returns>
        public static Vector<float> Normal(this Vector<float> v)
        {
            if (v.Count != 2) //only defined for 2-dimensions
                return null;
            return Vector<float>.Build.DenseOfArray(new[] {v.At(1), -v.At(0)});
        }

        /// <summary>
        /// calculates the intersection point between two straights of the type g(x)= p+ x*v
        /// </summary>
        /// <param name="v1">the direction vector of the first straight</param>
        /// <param name="v2">the direction vector of the second straight</param>
        /// <param name="p2">the starting point of the second straight</param>
        /// <param name="p1">the starting point of the first straight. if no value is given it is (0|0)</param>
        /// <returns>the intersection point of the two straights</returns>
        public static Vector<float> IntersectionPoint(this Vector<float> v1, Vector<float> v2,
            Vector<float> p2, Vector<float> p1 = null)
        {
            if (p1 == null)
                p1 = Vector<float>.Build.Dense(2);
            if (v1.GetMagnitude().Equals(0f) || v2.GetMagnitude().Equals(0f))
                return null;
            //p1 + v1 * k = p2 + v2 * j
            //rearragne equation to j and calculate it
            float j = ((p1.At(0) - p2.At(0))/v2.At(0) + (v1.At(0)/v2.At(0))*((p2.At(1) - p1.At(1))/v1.At(1)))/
                      (1 - (v1.At(0)*v2.At(1)/(v2.At(0)*v1.At(1))));
            if (float.IsNaN(j)) //if it don't works because of a division through zero
            {
                Vector<float> buffer = p1;
                p1 = p2;
                p2 = buffer;
                buffer = v1;
                v1 = v2;
                v2 = buffer;
                j = ((p1.At(0) - p2.At(0)) / v2.At(0) + (v1.At(0) / v2.At(0)) * ((p2.At(1) - p1.At(1)) / v1.At(1))) /
                          (1 - (v1.At(0) * v2.At(1) / (v2.At(0) * v1.At(1))));
            }
            //intersection point = p2 + v2 * j
            return Vector<float>.Build.DenseOfArray(new[] {p2.At(0) + v2.At(0)*j, p2.At(1) + v2.At(1)*j});
        }

        /// <summary>
        /// calculates the angle between two 2-dimensional vectors
        /// </summary>
        /// <param name="v1">the first vector</param>
        /// <param name="v2">the second vector</param>
        /// <returns>the angle between the vectors</returns>
        public static float GetAngle(this Vector<float> v1, Vector<float> v2)
        {
            if (v1.GetMagnitude().Equals(0) || v2.GetMagnitude().Equals(0))
                return 0;
            float value = (v1.At(0)*v2.At(0) + v1.At(1) * v2.At(1))/(v1.GetMagnitude()*v2.GetMagnitude());
            //the arccos is defined between -1 and 1
            if (value > 1)
                value = value - ((int) value)*2;
            if (value < -1)
                value = value + ((int) value)*2;
            return Convert.ToSingle(Math.Acos(value));
        }

        /// <summary>
        /// get the lenght of the vector
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float GetMagnitude(this Vector<float> v)
        {
            return Convert.ToSingle(v.L2Norm());
        }
    }
}
