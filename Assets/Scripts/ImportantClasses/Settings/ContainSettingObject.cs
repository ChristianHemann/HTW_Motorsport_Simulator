using System;
using System.Collections.Generic;

namespace ImportantClasses
{
    /// <summary>
    /// is used to provide easy access to the Name which is defined in the ContainSettingsAttribute of an object
    /// </summary>
    public class ContainSettingObject
    {
        public string Name { get; set; } //The name which is defined in the ContainSettingsAttribute
        public object Obj { get; set; } //The object under the ContainSettingsAttribute
        public Type ParentType { get; set; } //The parent Type of the object
        public string Path { get; set; } //the path where the object is saved on the filesystem
        public Dictionary<string, object> NamesHierachy { get; set; } //saves all the MenuItems and Settings to accelerate the finding of a specific name

        /// <summary>
        /// is used to provide easy access to the Name which is defined in the ContainSettingsAttribute of an object
        /// </summary>
        /// <param name="name">The name which is defined in the ContainSettingsAttribute</param>
        /// <param name="obj">The object under the ContainSettingsAttribute</param>
        /// <param name="parentType">The parent Type of the object</param>
        /// <param name="path">the path where the object is saved on the filesystem</param>
        public ContainSettingObject(string name, object obj, Type parentType, string path)
        {
            Name = name;
            Obj = obj;
            ParentType = parentType;
            Path = path;
            NamesHierachy = new Dictionary<string, object>();
        }
    }
}
