using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses
{
    /// <summary>
    /// Defines an Entrance-Point for the Settings-functions to search
    /// It is only allowed to static objects. Otherwise the objects cannot be found.
    /// To save the object a [XmlElement]-Attribute is also needed
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ContainSettingsAttribute : Attribute
    {
        public readonly string Name;
        public readonly bool ShowAsMenuItem;

        /// <summary>
        /// Defines an Entrance-Point for the Settings-functions to search
        /// It is only allowed to static objects. Otherwise the objects cannot be found.
        /// To save the object a [XmlElement]-Attribute is also needed
        /// </summary>
        /// <param name="name">The Name which will be shown in the Menu</param>
        /// <param name="showAsMenuItem">Defines wheather this object will be shown as a Menu Item. If the Value is set to false, the objects children are still visible. The DefualtValue is true.</param>
        public ContainSettingsAttribute(string name, bool showAsMenuItem = true)
        {
            this.Name = name;
            this.ShowAsMenuItem = showAsMenuItem;
        }
    }
}
