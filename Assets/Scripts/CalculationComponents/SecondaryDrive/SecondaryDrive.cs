using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;
using Output;

namespace CalculationComponents
{
    public class SecondaryDrive : ICalculationComponent
    {
        /// <summary>
        /// the transmission of the secondary drive
        /// </summary>
        [Setting("Transmission (rpmIn/rpmOut)")]
        public float Transmission { get; set; }

        /// <summary>
        /// the moment of inertia of the secondary drive
        /// </summary>
        [Setting("Polar area moment of Inertia (m^4)")]
        public float InertiaTorque { get; set; }

        private SecondaryDriveOutput _actualCalculation;

        public SecondaryDrive()
        {
            _actualCalculation = new SecondaryDriveOutput();
            Transmission = 1;
            InertiaTorque = 0;
        }

        public void Calculate()
        {
            _actualCalculation.Torque = GearBoxOutput.LastCalculation.Torque*Transmission;
        }

        public void StopCalculation()
        {
            //the calculation is too short as it would worth to stop it
        }

        public void StoreResult()
        {
            throw new NotImplementedException();
        }

        public void CalculateBackwards()
        {
            throw new NotImplementedException();
        }

        public event CalculationReadyDelegate OnCalculationReady;
    }
}
