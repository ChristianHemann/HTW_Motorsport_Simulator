
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
        /// gets an orthogonal Vector of the original 2-dimensional Vector
        /// </summary>
        /// <param name="v">the original Vector</param>
        /// <returns></returns>
        public static Vector<float> Normal(this Vector<float> v)
        {
            if (v.Count == 0)
                return null;
            Vector<float> normal;
            if (v.At(1).Equals(0f))
                normal = Vector<float>.Build.DenseOfArray(new[] {0f, 1f});
            else if (v.At(0).Equals(0f))
                normal = Vector<float>.Build.DenseOfArray(new[] { 1f, 0f });
            else
                normal = Vector<float>.Build.DenseOfArray(new[] { 1f, v.At(0)/v.At(1) });

            return normal;
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
            //p1 + v1 * k = p2 + v2 * j
            //rearragne equation to j and calculate it
            float j = ((p1.At(0) - p2.At(0))/v2.At(0) + (v1.At(0)/v2.At(0))*((p2.At(1) - p1.At(1))/v1.At(1)))/
                      (1 - (v1.At(0)*v2.At(1)/(v2.At(0)*v1.At(1))));
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
            return (float) Math.Acos((v1.At(0)*v2.At(0) + v1.At(1) + v2.At(1))/(v1.GetMagnitude()*v2.GetMagnitude()));
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
