using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses
{
    /// <summary>
    /// Defines a Property to be shown as a Setting in the Menu
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingAttribute : Attribute
    {
        public readonly object DefaultValue;
        public readonly string Name;
        public readonly double MinValue, MaxValue;
        public readonly sbyte DecimalPlaces;

        /// <summary>
        /// Defines a Property to be shown as a Setting in the Menu
        /// </summary>
        /// <param name="name">The Name which will be shown in the Menu</param>
        /// <param name="defaultValue">The defaultValue for this property. If it is not given the DefaultValue will be null</param>
        /// <param name="minValue">If the Value is numeric, the minimum Value can be given. When the maximum Value is lower than the minimum value or equal, there is no limitation. The DefaultValue is 0</param>
        /// <param name="maxValue">If the Value is numeric, the maximum Value can be given. When the maximum Value is lower than the minimum value or equal, there is no limitation. the DefaultValue is 0</param>
        /// <param name="decimalPlaces">If the Value is Numeric: Defines the accuracy, which can be set for this property. If the Value is negative, the predecimal Places are meant. The DefaultValue is 0</param>
        public SettingAttribute(string name, object defaultValue = null, double minValue = 0, double maxValue = 0, sbyte decimalPlaces = 0)
        {
            DefaultValue = defaultValue;
            Name = name;
            DecimalPlaces = decimalPlaces;
            MinValue = minValue;
            MaxValue = maxValue;
        }
    }
}
