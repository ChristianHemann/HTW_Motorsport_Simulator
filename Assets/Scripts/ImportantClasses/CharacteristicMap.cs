using System;
using System.Xml.Serialization;

namespace ImportantClasses
{
    /// <summary>
    /// linear Interpolation in 3D
    /// </summary>
    public class CharacteristicMap
    {
        [XmlArray]
        public double[] X
        {
            get { return _x; }
            set
            {
                if (_x == null) _x = value;
            }
        }

        [XmlArray]
        public double[] Y
        {
            get { return _y; }
            set
            {
                if (_y == null) _y = value;
            }
        }

        [XmlArray]
        public double[][] Values
        {
            get { return _values; }
            set
            {
                if (_values == null) _values = value;
            }
        }

        [XmlIgnore]
        private double[] _x, _y;
        [XmlIgnore]
        private double[][] _values;

        private CharacteristicMap() //Only to provide Xml-Functions
        {
        }

        /// <summary>
        /// linear Interpolation in 3D
        /// </summary>
        /// <param name="x">the points in x-Direction</param>
        /// <param name="y">the points in y-Direction</param>
        /// <param name="values">the values at each intersection point of [x-values][y-value]</param>
        public CharacteristicMap(double[] x, double[] y, double[][] values)
        {
            //sort the X-values ascending
            bool isAscending = false;
            while (!isAscending)
            {
                for (int i = 1; i < x.Length; i++)
                {
                    if (x[i - 1] > x[i]) //compare whether each value is bigger than the previous
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (x[j] > x[i]) //if the value at j is bigger than the value at i, the correct position was found
                            {
                                //exchange the values;
                                double xI = x[i];
                                double xJ = x[j];
                                x[i] = xJ;
                                x[j] = xI;

                                double[] valuesI = values[i];
                                double[] valuesJ = values[j];
                                values[i] = valuesJ;
                                values[j] = valuesI;
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
            //sort the Y-values ascending
            isAscending = false;
            while (!isAscending)
            {
                for (int i = 1; i < y.Length; i++)
                {
                    if (y[i - 1] > y[i]) //compare whether each value is bigger than the previous
                    {
                        for (int j = 0; j < i; j++)
                        {
                            if (y[j] > y[i]) //if the value at j is bigger than the value at i, the correct position was found
                            {
                                //exchange the values;
                                double yI = y[i];
                                double yJ = y[j];
                                y[i] = yJ;
                                y[j] = yI;

                                double valueI, valueJ;
                                for (int k = 0; k < y.Length; k++)
                                {
                                    valueJ = values[k][j];
                                    valueI = values[k][i];
                                    values[k][j] = valueI;
                                    values[k][i] = valueJ;
                                }
                                break;
                            }
                        }
                        break;
                    }

                    if (y[i - 1].Equals(y[i])) //if a X-value exists twice
                        throw new ArgumentException("LinearInterpolation: it is forbidden, that a X-value appears twice");

                    if (i == y.Length - 1) //if the loop has run to its end
                        isAscending = true;
                }
            }

            _x = x;
            _y = y;
            _values = values;
        }

        /// <summary>
        /// Interpolates a value on the CharacteristicMap at the given point
        /// </summary>
        /// <param name="x">Y-coordinate of the point</param>
        /// <param name="y">Y-coordinate of the point</param>
        /// <returns>The interpolated value</returns>
        public double Interpolate(double x, double y)
        {
            if(x<_x[0]||y<_y[0]) //If the value is too small
                throw new ArgumentOutOfRangeException("the given x or y value to interpolate is out of the CharacteristicMap. (At Least One Value is too small");

            int posX = _x.Length + 1, posY = _y.Length + 1;
            for (int i = 1; i < _x.Length; i++)
            {
                if (_x[i] >= x)
                {
                    posX = i;
                    break;
                }
            }
            for (int i = 1; i < _y.Length; i++)
            {
                if (_y[i] >= y)
                {
                    posY = i;
                    break;
                }
            }

            if(posY == _y.Length+1 || posX == _x.Length+1) //If the value were not changed it is out of Range
                throw new ArgumentOutOfRangeException("the given x or y value to interpolate is out of the CharacteristicMap. (At Least One Value is too big");

            //interpolate two values in x-direction (at posY and posY-1) and then interpolate between them
            //interpolation Formula:((z1-z0) / (x1-x0)) * (x-x0) + z0
            double z0 = ((_values[posX][posY - 1] - _values[posX - 1][posY - 1]) / (_x[posX] - _x[posX - 1])) *
                        (x - _x[posX - 1]) + _values[posX - 1][posY - 1];
            double z1 = ((_values[posX][posY] - _values[posX - 1][posY]) / (_x[posX] - _x[posX - 1])) *
                        (x - _x[posX - 1]) + _values[posX - 1][posY];
            //interpolate between the values in y-direction
            double z = ((z1 - z0)/(_y[posY] - _y[posY - 1]))*(y - _y[posY - 1]) + z0;
            return z;
        }
    }
}
