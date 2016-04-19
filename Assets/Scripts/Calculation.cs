using UnityEngine;
using System.Collections;
using MathNet.Numerics.Interpolation;

public class Calculation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
        Simulator.CalculationController.test();
    }

    public static double test()
    {
        //test the Library
        double[] x = new double[] { 0.0, 1.0 };
        double[] y = new double[] { 2.0, 5.0 };
        CubicSpline spline = CubicSpline.InterpolateNaturalInplace(x, y);
        return spline.Interpolate(0.5);
    }
}
