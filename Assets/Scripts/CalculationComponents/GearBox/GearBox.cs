using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;
using Output;
using UnityEngine;

namespace CalculationComponents
{
    /// <summary>
    /// calculates an gearBox. Will be initialized with the standard values
    /// </summary>
    public class GearBox : ICalculationComponent
    {
        //Settings
        [Setting("Efficency", 0.9, 0.0, 1.0, 3)]
        public float efficency;

        [Setting("Number of Gears", 4)]
        public byte gears {
            get { return _gears; }
            set
            {
                _gears = value;
                
            }
        }

        [Setting("Transmission for each gear (rpmIn/rpmOut)")]
        public float[] transmissions;

        /// <summary>
        /// sets the gear in which the car is
        /// </summary>
        public sbyte Gear { get; set; }

        private byte _gears;

        private GearBoxOutput actualCalculation; //the result of the actual done calculation

        /// <summary>
        /// calculates an gearBox. Will be initialized with the standard values
        /// </summary>
        public GearBox()
        {
            gears = 4;
            efficency = 0.9f;
            actualCalculation = new GearBoxOutput();
            transmissions = new float[gears];
        }

        /// <summary>
        /// calculate the GearBoxOutputs with the actual inputs
        /// </summary>
        public void Calculate()
        {
            if (Gear == 0)
                actualCalculation.torque = 0;
            else
                actualCalculation.torque = EngineOutput.LastCalculation.torque*efficency*transmissions[Gear-1];

            if (OnCalculationReady != null)
                OnCalculationReady();
        }

        /// <summary>
        /// stops the calculation if it necessary to abort it
        /// </summary>
        public void StopCalculation()
        {
            //the function is so small, that it makes no sense
            return;
        }

        /// <summary>
        /// stores the result of the calculation to the GearBoxOutput class
        /// </summary>
        public void StoreResult()
        {
            GearBoxOutput.LastCalculation.torque = actualCalculation.torque;
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
