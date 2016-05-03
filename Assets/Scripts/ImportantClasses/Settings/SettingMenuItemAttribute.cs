using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses
{
    /// <summary>
    /// Defines a Property to be shown as a Menu Item in the Menu
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SettingMenuItemAttribute : Attribute
    {
        public readonly string Name;

        /// <summary>
        /// Defines a Property to be shown as a Menu Item in the Menu
        /// </summary>
        /// <param name="name">The Name which will be shown in the Menu</param>
        public SettingMenuItemAttribute(string name)
        {
            Name = name;
        }
    }
}
