
namespace ImportantClasses
{
    /// <summary>
    /// Provides mathematic functions which are often needed
    /// </summary>
    public class MathHelper
    {
        /// <summary>
        /// ratio between two numerical values. The sum of both is 1.
        /// </summary>
        public class Ratio
        {
            /// <summary>
            /// The first vale of the ratio FirstValue:SecondValue
            /// </summary>
            public double FirstValue
            {
                get { return _firstValue; }
                set
                {
                    _firstValue = value;
                    _secondValue = 1 - value;
                }
            }

            /// <summary>
            /// The second value of the ratio FirstValue:SecondValue
            /// </summary>
            public double SecondValue
            {
                get { return _secondValue; }
                set
                {
                    _secondValue = value;
                    _firstValue = 1 - value;
                }
            }

            private double _firstValue;
            private double _secondValue;

            /// <summary>
            /// ratio between two numerical values. The sum of both is 1.
            /// </summary>
            /// <param name="firstValue">The first value of the ratio FirstValue:SecondValue</param>
            /// <param name="secondValue">The second value of the ratio FirstValue:SecondValue</param>
            public Ratio(double firstValue, double secondValue)
            {
                ReCalculate(firstValue, secondValue);
            }

            /// <summary>
            /// sets the ratio as the same ratio as firstValue:secondValue but normalize it so that the sum is 1
            /// </summary>
            /// <param name="firstValue">The first value of the ratio FirstValue:SecondValue</param>
            /// <param name="secondValue">The second value of the ratio FirstValue:SecondValue</param>
            /// <returns>the new normalized ratio</returns>
            public Ratio ReCalculate(double firstValue, double secondValue)
            {
                double divisor = firstValue + secondValue*0.5;
                if (!divisor.Equals(0))
                {
                    _firstValue = firstValue/divisor;
                    _secondValue = secondValue/divisor;
                }
                return this;
            }
        }
    }
}
