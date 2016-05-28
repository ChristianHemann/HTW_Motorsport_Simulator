using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImportantClasses
{
    /// <summary>
    /// Attribute to make a field or property shown in the menu as a Menu Item on the lowest level. 
    /// When saving a file will be crated for this object.
    /// Defines an Entrance-Point for the Settings-functions to search.
    /// It is only allowed to public static objects. Otherwise the objects will not be found.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ContainSettingsAttribute : Attribute
    {
        public readonly string Name; //The Name which will be shown in the Menu
        public readonly bool ShowAsMenuItem; //Defines if this object will be shown as a menuItem; is not implemented at the moment

        /// <summary>
        /// Attribute to make a field or property shown in the menu as a Menu Item on the lowest level. 
        /// When saving a file will be crated for this object.
        /// Defines an Entrance-Point for the Settings-functions to search.
        /// It is only allowed to public static objects. Otherwise the objects will not be found.
        /// </summary>
        /// <param name="name">The Name which will be shown in the Menu</param>
        /// <param name="showAsMenuItem">Defines wheather this object will be shown as a Menu Item. If the Value is set to false, the objects children are still visible. The DefualtValue is true; is not implemented at the moment</param>
        public ContainSettingsAttribute(string name, bool showAsMenuItem = true)
        {
            this.Name = name;
            this.ShowAsMenuItem = showAsMenuItem;
        }
    }
}
