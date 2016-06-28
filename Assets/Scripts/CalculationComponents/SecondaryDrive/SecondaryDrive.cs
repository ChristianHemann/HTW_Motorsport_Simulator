using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;
using Output;

namespace CalculationComponents
{
    /// <summary>
    /// The Seconrady Drive of the vehicle
    /// </summary>
    public class SecondaryDrive : ICalculationComponent
    {
        /// <summary>
        /// the transmission of the secondary drive
        /// </summary>
        [Setting("Transmission (rpmIn/rpmOut)")]
        public float Transmission { get; set; }

        /// <summary>
        /// the polar area moment of inertia of the secondary drive
        /// </summary>
        [Setting("Polar area moment of inertia (m^4)")]
        public float InertiaTorque { get; set; }

        private readonly SecondaryDriveOutput _actualCalculation;

        /// <summary>
        /// The Seconrady Drive of the vehicle
        /// </summary>
        public SecondaryDrive()
        {
            _actualCalculation = new SecondaryDriveOutput();
            Transmission = 1;
            InertiaTorque = 0;
        }

        /// <summary>
        /// Calculates the output torque of the secondary drive
        /// </summary>
        public void Calculate()
        {
            _actualCalculation.Torque = GearBoxOutput.LastCalculation.Torque*Transmission;
        }

        /// <summary>
        /// stops the actual running calculation
        /// </summary>
        public void StopCalculation()
        {
            //the calculation is too short as it would worth to stop it
        }

        /// <summary>
        /// stores the results of the Calculate-function
        /// </summary>
        public void StoreResult()
        {
            SecondaryDriveOutput.LastCalculation.Torque = _actualCalculation.Torque;
        }

        /// <summary>
        /// invoke when the other parts of the car are calculated. Calculates the output rpm
        /// </summary>
        public void CalculateBackwards()
        {
            //calculate rpm when the car is calculated
        }

        /// <summary>
        /// will be triggered when the calculation is done
        /// </summary>
        public event CalculationReadyDelegate OnCalculationReady;
    }
}
