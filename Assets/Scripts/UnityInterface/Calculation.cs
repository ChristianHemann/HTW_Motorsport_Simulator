using UnityEngine;
using System.Collections;
using MathNet.Numerics.Interpolation;
using Simulator;

namespace UnityInterface
{
    public class Calculation : MonoBehaviour
    {
        private Simulator.CalculationController controller;

        // Initialization
        void Start()
        {
            //controller = new CalculationController();
            //controller.Initialize();
        }

        //called periodically
        void FixedUpdate()
        {
            //controller.Calculate();
        }

        // Update is called once per frame
        void Update()
        {

        }

        //quitting
        void OnApplicationQuit()
        {
            //controller.Terminate();
        }
    }
}
