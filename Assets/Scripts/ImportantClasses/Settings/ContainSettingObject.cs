using System;

namespace ImportantClasses
{
    /// <summary>
    /// This Class is used to provide easy access to the Name which is defined in the ContainSettingsAttribute of an object
    /// </summary>
    public class ContainSettingObject
    {
        public string Name;
        public object Obj;
        public Type ParentType;
        public string Path;

        public ContainSettingObject()
        {

        }

        public ContainSettingObject(string name, object obj, Type parentType, string path)
        {
            Name = name;
            Obj = obj;
            ParentType = parentType;
            Path = path;
        }
    }
}
