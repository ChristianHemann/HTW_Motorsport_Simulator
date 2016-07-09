using System;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace ImportantClasses
{
    /// <summary>
    /// a vector in 3-dimensional space
    /// </summary>
    public class Vector3
    {
        /// <summary>
        /// the x-coordinate of the vector (horizontal; left/right)
        /// </summary>
        public float X
        {
            get { return _vector[0]; }
            set { _vector[0] = value; }
        }

        /// <summary>
        /// the y-coordinate of the vector (horizontal; forwards/backwards)
        /// </summary>
        public float Y
        {
            get { return _vector[1]; }
            set { _vector[1] = value; }
        }

        /// <summary>
        /// the x-coordinate of the vector (vertical; up/down)
        /// </summary>
        public float Z
        {
            get { return _vector[2]; }
            set { _vector[2] = value; }
        }

        /// <summary>
        /// the lenght of the vector
        /// </summary>
        public float Magnitude { get { return Convert.ToSingle(_vector.L2Norm()); } }

        /// <summary>
        /// the vector which is wrapped by this class
        /// </summary>
        private readonly Vector<float> _vector;

        /// <summary>
        /// a vector in 3-dimensional space
        /// </summary>
        public Vector3() : this(0f, 0f, 0f) { }

        /// <summary>
        /// a vector in 3-dimensional space
        /// </summary>
        /// <param name="x">the x-coordinate of the vector</param>
        /// <param name="y">the y-coordinate of the vector</param>
        /// <param name="z">the z-coordinate of the vector</param>
        public Vector3(float x, float y, float z)
        {
            _vector = Vector<float>.Build.DenseOfArray(new[] { x, y, z });
        }

        public override string ToString()
        {
            return
                new StringBuilder().Append("X: ").Append(X).Append("\nY: ").Append(Y)
                .Append("\nZ: ").Append(Z).ToString();
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
            if (obj is Vector3)
                other = ((Vector3)obj)._vector;
            else
                return false;

            if (_vector[0] > other[0] + delta || _vector[0] < other[0] - delta)
                return false;
            if (_vector[1] > other[1] + delta || _vector[1] < other[1] - delta)
                return false;
            if (_vector[2] > other[2] + delta || _vector[2] < other[2] - delta)
                return false;
            return true;
        }

        /// <summary>
        /// calculates a vector with the length of 1 and the same direction as this vector
        /// </summary>
        /// <returns>the vector with the normalized length</returns>
        public Vector3 Normalize()
        {
            Vector<float> buffer = _vector.Normalize(2);
            return new Vector3(buffer[0], buffer[1], buffer[2]);
        }

        public static implicit operator Vector3(Vector2 vector)
        {
            return vector.ToVector3();
        }

        public static Vector3 operator +(Vector3 v1, Vector2 v2)
        {
            return v1 + v2.ToVector3();
        }

        public static Vector3 operator +(Vector2 v1, Vector3 v2)
        {
            return v2 + v1.ToVector3();
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.X, -v.Y, -v.Z);
        }

        public static Vector3 operator *(float factor, Vector3 v)
        {
            return v * factor;
        }

        public static Vector3 operator *(Vector3 v, float factor)
        {
            return new Vector3(v.X * factor, v.Y * factor, v.Z * factor);
        }

        public static Vector3 operator *(double factor, Vector3 v)
        {
            return v * (float)factor;
        }

        public static Vector3 operator *(Vector3 v, double factor)
        {
            return v * (float)factor;
        }

        public static Vector3 operator /(Vector3 v, float divisor)
        {
            return new Vector3(v.X / divisor, v.Y / divisor, v.Z / divisor);
        }
    }
}
