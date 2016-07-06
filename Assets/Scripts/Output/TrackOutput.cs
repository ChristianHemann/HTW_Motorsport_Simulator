using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CalculationComponents;

namespace Output
{
    /// <summary>
    /// saves the Calculated Values of the Track. Actually the class is not needed
    /// </summary>
    public class TrackOutput
    {
        /// <summary>
        /// the results of the last Calculation step
        /// </summary>
        public TrackOutput LastCalculation
        {
            get
            {
                if(_lastCalculation ==null)
                    _lastCalculation = new TrackOutput();
                return _lastCalculation;
            }
            set { _lastCalculation = value; }
        }
        private TrackOutput _lastCalculation;
    }
}
