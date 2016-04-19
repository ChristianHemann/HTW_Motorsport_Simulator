using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathNet.Numerics.Interpolation;

namespace Simulator
{
    public class CalculationController
    {
        public static double test()
        {
            //test the Library
            double[] x = new double[] { 0.0, 1.0 };
            double[] y = new double[] { 2.0, 5.0 };
            CubicSpline spline = CubicSpline.InterpolateNaturalInplace(x, y);
            return spline.Interpolate(0.5);
        }
    }
}
