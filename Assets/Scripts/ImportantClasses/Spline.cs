using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using MathNet.Numerics.Interpolation;

namespace ImportantClasses
{
    /// <summary>
    /// Interpolates cubic piecewise between the given points
    /// this class is a wrapper to provide .NET XML Functions to the MathDotNet Library
    /// </summary>
    public class Spline
    {
        [XmlArray]
        public double[] X
        {
            get { return _x; }
            set
            {
                if (_x == null) //this should only be entered when the Spline was created from xml
                {
                    _x = value;
                }
            }
        }
        [XmlArray]
        public double[] Y
        {
            get { return _y; }
            set
            {
                if (_y == null) //this should only be entered when the Spline was created from xml
                {
                    _y = value;
                    spline = CubicSpline.InterpolateNaturalInplace(_x, _y);
                }
            }
        }
        public string NameX { get; set; }
        public string NameY { get; set; }

        [XmlIgnore]
        private double[] _x;
        [XmlIgnore]
        private double[] _y;
        [XmlIgnore]
        private CubicSpline spline;

        /// <summary>
        /// this constructor is to provide Xml-Functions
        /// </summary>
        private Spline()
        {

        }

        /// <summary>
        /// creats a new function which interpolates cubic piecewise between the points
        /// </summary>
        public Spline(double[] x, double[] y)
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
                                listX.Insert(j, x[i]);
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
            spline = CubicSpline.InterpolateNaturalInplace(x, y);
        }

        /// <summary>
        /// interpolates the Y-value at the given point
        /// </summary>
        /// <param name="point">the point to interpolate</param>
        /// <returns>the interpolated value</returns>
        public double Interpolate(double point)
        {
            return spline.Interpolate(point);
        }
    }
}
