using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using MathNet.Numerics.LinearAlgebra;

namespace ImportantClasses
{
    public class Vector2
    {
        private Vector<float> _vector;

        public float X {
            get { return _vector[0]; }
            set { _vector[0] = value; }
        }

        public float Y
        {
            get { return _vector[1]; }
            set { _vector[1] = value; }
        }

        public float Magnitude { get { return Convert.ToSingle(_vector.L2Norm()); } }

        public Vector2(float x, float y)
        {
            _vector = Vector<float>.Build.DenseOfArray(new[] {x, y});
        }

        public Vector2() : this(0f, 0f) { }

        public override string ToString()
        {
            return
                new StringBuilder().Append("X: ").Append(X.ToString()).Append("\nY: ").Append(Y.ToString()).ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj, 0f);
        }

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

        public Vector2 GetIntersectionPoint(Vector2 otherDirection, Vector2 otherStartingPoint,
            Vector2 thisStartingPoint)
        {
            return GetIntersectionPoint(this, otherDirection, otherStartingPoint, thisStartingPoint);
        }

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
            //intersection point = p2 + v2 * j
            return new Vector2(p2.X + v2.X*j, p2.Y + v2.Y*j);
        }

        public Vector2 Normal()
        {
            return new Vector2(Y, -X);
        }

        public Vector2 Normalize()
        {
            Vector<float> buffer = _vector.Normalize(2);
            return new Vector2(buffer[0], buffer[1]);
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
