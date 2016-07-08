using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using MathNet.Numerics.Interpolation;

namespace ImportantClasses
{
    /// <summary>
    /// Interpolates linear piecewise between the given points
    /// this class is a wrapper to provide .NET XML Functions to the MathDotNet Library
    /// </summary>
    public class LinearInterpolation
    {
        /// <summary>
        /// the x-values of the points to interpolate between
        /// </summary>
        [XmlArray]
        public double[] X {
            get { return _x; }
            set { 
                if (_x == null) //this should only be entered when the Spline was created from xml
                {
                    _x = value;
                }
            }
        }

        /// <summary>
        /// the y-values of the points to interpolate betweeen
        /// </summary>
        [XmlArray]
        public double[] Y
        {
            get { return _y; }
            set
            {
                if (_y == null) //this should only be entered when the Spline was created from xml
                {
                    _y = value;
                    _spline = LinearSpline.InterpolateInplace(_x, _y);
                }
            }
        }

        [XmlIgnore]
        private double[] _x;
        [XmlIgnore]
        private double[] _y;
        [XmlIgnore]
        private LinearSpline _spline;

        /// <summary>
        /// this constructor is to provide Xml-Functions
        /// </summary>
        private LinearInterpolation()
        {
            
        }

        /// <summary>
        /// Interpolates linear piecewise between the given points
        /// this class is a wrapper to provide .NET XML Functions to the MathDotNet Library
        /// </summary>
        /// <param name="x">there are the supporting points</param>
        /// <param name="y">the value at each supporting point. Must have the same length as the x-values</param>
        public LinearInterpolation(double[] x, double[] y)
        {
            //sort the values ascending
            bool isAscending = false;
            while (!isAscending)
            {
                for (int i = 1; i < x.Length; i++)
                {
                    if (x[i - 1] > x[i]) //compare whether each value is bigger than the previous
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (x[j] > x[i]) //if the value at j is bigger than the value at i the correct position was found
                            {
                                //displace the value;
                                List<double> listX = x.ToList();
                                listX.Remove(x[i]);
                                listX.Insert(j,x[i]);
                                x = listX.ToArray();
                                List<double> listY = y.ToList();
                                listY.Remove(y[i]);
                                listY.Insert(j, y[i]);
                                y = listY.ToArray();
                                break;
                            }
                        }
                        break;
                    }

                    if (x[i - 1].Equals(x[i])) //if a X-value exists twice
                        throw new ArgumentException("LinearInterpolation: it is forbidden, that a X-value appears twice");

                    if (i == x.Length - 1) //if the loop has run to its end
                        isAscending = true;
                }
            }
            this._x = x;
            this._y = y;
            _spline = LinearSpline.InterpolateInplace(x, y);
        }

        /// <summary>
        /// interpolates the Y-value at the given point
        /// </summary>
        /// <param name="point">the point to interpolate</param>
        /// <returns>the interpolated value</returns>
        public double Interpolate(double point)
        {
            return _spline.Interpolate(point);
        }
    }
}
