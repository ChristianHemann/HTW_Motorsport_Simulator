using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace ImportantClasses
{
    /// <summary>
    /// vector in two dimensional space
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// the vector which is wrapped by this class
        /// </summary>
        private Vector<float> _vector;

        /// <summary>
        /// the x-coordinate of the vector
        /// </summary>
        public float X {
            get { return _vector[0]; }
            set { _vector[0] = value; }
        }

        /// <summary>
        /// the y-coordinate of the vector
        /// </summary>
        public float Y
        {
            get { return _vector[1]; }
            set { _vector[1] = value; }
        }

        /// <summary>
        /// the lenght of the vector
        /// </summary>
        public float Magnitude { get { return Convert.ToSingle(_vector.L2Norm()); } }

        /// <summary>
        /// vector in two dimensional space
        /// </summary>
        /// <param name="x">the x-coordinate of the vector</param>
        /// <param name="y">the y-coordinate of the vector</param>
        public Vector2(float x, float y)
        {
            _vector = Vector<float>.Build.DenseOfArray(new[] {x, y});
        }

        /// <summary>
        /// vector in two dimensional space with a length of 0
        /// </summary>
        public Vector2() : this(0f, 0f) { }

        public override string ToString()
        {
            return
                new StringBuilder().Append("X: ").Append(X.ToString()).Append("\nY: ").Append(Y.ToString()).ToString();
        }

        /// <summary>
        /// checks if the this vector and the object are having the same values
        /// </summary>
        /// <param name="obj">the object to compare this vector with</param>
        /// <returns>true if the object is a vector with the same coordinated</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj, 0f);
        }

        /// <summary>
        /// checks if the this vector and the object are having the same values
        /// </summary>
        /// <param name="obj">>the object to compare this vector with</param>
        /// <param name="delta">the maximum difference between the values of the vectors. A value around 1E-5 is appropriate</param>
        /// <returns>true if the object is a vector with the same coordinated</returns>
        public bool Equals(object obj, float delta)
        {
            Vector<float> other;
            if (obj is Vector2)
                other = ((Vector2) obj)._vector;
            else
                return false;

            if (_vector[0] > other[0] + delta || _vector[0] < other[0] - delta)
                return false;
            if (_vector[1] > other[1] + delta || _vector[1] < other[1] - delta)
                return false;
            return true;
        }

        /// <summary>
        /// calculates the angel between 2 vectors
        /// </summary>
        /// <param name="other">the other vector</param>
        /// <returns>the angle between the 2 vectors in radiant</returns>
        public float GetEnclosedAngle(Vector2 other)
        {
            if (Magnitude.Equals(0) || other.Magnitude.Equals(0))
                return 0;
            float buffer = (X * other.X + Y * other.Y) / (Magnitude * other.Magnitude);
            //the arccos is defined between -1 and 1
            if (buffer > 1)
                buffer = buffer - ((int)buffer) * 2;
            if (buffer < -1)
                buffer = buffer + ((int)buffer) * 2;
            return Convert.ToSingle(Math.Acos(buffer));
        }

        /// <summary>
        /// Calculates the intersection point between 2 straights
        /// </summary>
        /// <param name="otherDirection">the direction vector of the second straight</param>
        /// <param name="otherStartingPoint">the stationary vector of the second straight</param>
        /// <param name="thisStartingPoint">the stationary vector of the first straight</param>
        /// <returns>the intersection point of the two vectors or null if they have no intersection point</returns>
        public Vector2 GetIntersectionPoint(Vector2 otherDirection, Vector2 otherStartingPoint,
            Vector2 thisStartingPoint)
        {
            return GetIntersectionPoint(this, otherDirection, otherStartingPoint, thisStartingPoint);
        }

        /// <summary>
        /// Calculates the intersection point between 2 straights
        /// </summary>
        /// <param name="thisDirection">the direction vector of the first straight</param>
        /// <param name="otherDirection">the direction vector of the second straight</param>
        /// <param name="otherStartingPoint">the stationary vector of the second straight</param>
        /// <param name="thisStartingPoint">the stationary vector of the first straight</param>
        /// <returns>the intersection point of the two vectors or null if they have no intersection point</returns>
        public static Vector2 GetIntersectionPoint(Vector2 thisDirection, Vector2 otherDirection, Vector2 otherStartingPoint, Vector2 thisStartingPoint)
        {
            if (thisDirection.Magnitude.Equals(0f) || otherDirection.Magnitude.Equals(0f))
                return null;
            //just write it shorter
            Vector2 p1 = thisStartingPoint, p2 = otherStartingPoint, v1 = thisDirection, v2 = otherDirection;
            //p1 + v1 * k = p2 + v2 * j
            //rearragne equation to j and calculate it
            float j = ((p1.X - p2.X) / v2.X + (v1.X / v2.X) * ((p2.Y - p1.Y) / v1.Y)) /
                      (1 - (v1.X * v2.Y / (v2.X * v1.Y)));
            if (float.IsNaN(j)) //if it don't works because of a division through zero
            {
                Vector2 buffer = p1;
                p1 = p2;
                p2 = buffer;
                buffer = v1;
                v1 = v2;
                v2 = buffer;
                j = ((p1.X - p2.X) / v2.X + (v1.X / v2.X) * ((p2.Y - p1.Y) / v1.Y)) /
                      (1 - (v1.X * v2.Y / (v2.X * v1.Y)));
            }
            if (float.IsNaN(j)) //the vectors have no intersection point
                return null;
            //intersection point = p2 + v2 * j
            return new Vector2(p2.X + v2.X*j, p2.Y + v2.Y*j);
        }

        /// <summary>
        /// the vector which is orthogonal on this vector. Its direction is to the right side of this vector
        /// </summary>
        /// <returns>the orthogonal vector</returns>
        public Vector2 Normal()
        {
            return new Vector2(Y, -X);
        }

        /// <summary>
        /// calculates a vector with the length of 1 and the same direction as this vector
        /// </summary>
        /// <returns>the vector with the normalized length</returns>
        public Vector2 Normalize()
        {
            Vector<float> buffer = _vector.Normalize(2);
            return new Vector2(buffer[0], buffer[1]);
        }

        /// <summary>
        /// Turns a vector on the given angle
        /// </summary>
        /// <param name="angle">the angle to turn in radiant</param>
        /// <returns>the turned vector</returns>
        public Vector2 Turn(double angle)
        {
            float x = (float)(X * Math.Cos(angle) + Y * Math.Sin(angle));
            float y = (float)(Y * Math.Cos(angle) + X * Math.Sin(angle));
            return new Vector2(x, y);
        }

        /// <summary>
        /// Converts the Vecotr2 into a Vector3 with the Z-coordinate 0
        /// </summary>
        /// <returns>the new Vector3</returns>
        public Vector3 ToVector3()
        {
            return new Vector3(X, Y, 0f);
        }

        public static explicit operator Vector2(Vector3 vector)
        {
            return new Vector2(vector.X, vector.Y);
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2 operator -(Vector2 v)
        {
            return new Vector2(-v.X, -v.Y);
        }

        public static Vector2 operator *(float factor, Vector2 v)
        {
            return v*factor;
        }

        public static Vector2 operator *(Vector2 v, float factor)
        {
            return new Vector2(v.X*factor, v.Y*factor);
        }

        public static Vector2 operator /(Vector2 v, float divisor)
        {
            return new Vector2(v.X/divisor, v.Y/divisor);
        }
    }
}
