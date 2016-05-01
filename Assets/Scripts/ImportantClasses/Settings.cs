using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace ImportantClasses
{
    /// <summary>
    /// Saves the Settings, read them when the Software is started and Provides Functions to show them in the GUI
    /// Singleton
    /// </summary>
    public class Settings
    {
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }
        private static Settings _instance;
        private List<ContainSettingObject> settingList = new List<ContainSettingObject>();
        private static string settingsPath = @"D:\Studium\SWE\";

        private Settings()
        {
            SearchForContainSettingsAttribute();
        }

        public object GetMenuItems(string[] names)
        {
            foreach (object obj in settingList)
            {
                //obj.GetType().GetCustomAttributes(typeof(SettingMenuItemAttribute), false)
            }
            return null;
        }

        /// <summary>
        /// saves all properties and fields which are marked as a setting to a file
        /// </summary>
        public static void SaveSettings()
        {
            if (!Directory.Exists(settingsPath))
                Directory.CreateDirectory(settingsPath);
            foreach (ContainSettingObject obj in Instance.settingList)
            {
                Xml.WriteXml(Path.Combine(settingsPath, obj.Name + ".xml"), obj.Obj);
            }
        }

        /// <summary>
        /// load all properties and fields which are marked as a setting from a file
        /// </summary>
        public static void LoadSettings()
        {
            if (!Directory.Exists(settingsPath))
                return; //If the Directory doesn't exist no settings can be loaded
            for (int i = 0; i < Instance.settingList.Count; i++)
            {
                ContainSettingObject actObj = Instance.settingList.ElementAt(i);
                //search in fields
                FieldInfo[] fieldInfos = actObj.ParentType.GetFields().ToArray();
                foreach (FieldInfo fieldInfo in fieldInfos)
                {
                    if (((ContainSettingsAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(ContainSettingsAttribute))).Name == actObj.Name)
                    {
                        object obj = Xml.ReadXml(Path.Combine(settingsPath, actObj.Name + ".xml"), actObj.Obj.GetType());
                        //update the reference to the object in the list and in the original class
                        fieldInfo.SetValue(null, obj);
                        actObj.Obj = obj;
                    }
                }

                //search in properties
                PropertyInfo[] propertyInfos = actObj.ParentType.GetProperties().ToArray();
                foreach (PropertyInfo propertyInfo in propertyInfos)
                {
                    if (((ContainSettingsAttribute) Attribute.GetCustomAttribute(propertyInfo, typeof(ContainSettingsAttribute))).Name == actObj.Name)
                    {
                        object obj = Xml.ReadXml(Path.Combine(settingsPath, actObj.Name + ".xml"), actObj.Obj.GetType());
                        //update the reference to the object in the list and in the original class
                        propertyInfo.SetValue(null, obj, null);
                        actObj.Obj = obj;
                    }
                }
            }
        }

        /// <summary>
        /// fills the settingList with all the objects which has the ContainSettingsAttribute
        /// </summary>
        void SearchForContainSettingsAttribute()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    //Add all fields to the List
                    FieldInfo[] fieldInfos = type.GetFields().Where(prop => prop.IsDefined(typeof(ContainSettingsAttribute), false)).ToArray();
                    foreach (FieldInfo info in fieldInfos)
                    {
                        ContainSettingsAttribute[] attrs = ((ContainSettingsAttribute[])
                                info.GetCustomAttributes(typeof(ContainSettingsAttribute), false));
                        if (attrs.Length != 1)
                            throw new IndexOutOfRangeException("The field " + info.Name + " has more than one attribute of the Type ContainSettingsAttribute");
                        if (info.IsStatic)
                            settingList.Add(new ContainSettingObject(attrs.First().Name, info.GetValue(null), type));
                    }

                    //Add all Properties to the List
                    PropertyInfo[] propertyInfos = type.GetProperties().Where(prop => prop.IsDefined(typeof(ContainSettingsAttribute), false)).ToArray();
                    foreach (PropertyInfo info in propertyInfos)
                    {
                        ContainSettingsAttribute[] attrs = ((ContainSettingsAttribute[])
                                info.GetCustomAttributes(typeof(ContainSettingsAttribute), false));
                        if (attrs.Length != 1)
                            throw new IndexOutOfRangeException("The property " + info.Name + " has more than one attribute of the Type ContainSettingsAttribute");
                        if (info.GetAccessors().First().IsStatic)
                            settingList.Add(new ContainSettingObject(attrs.First().Name, info.GetValue(null, null), type));
                    }
                }
            }
        }
    }

    /// <summary>
    /// This Class is used to provide easy access to the Name which is defined in the ContainSettingsAttribute of an object
    /// </summary>
    public class ContainSettingObject
    {
        public string Name;
        public object Obj;
        public Type ParentType;
        public ContainSettingObject()
        {
            
        }

        public ContainSettingObject(string name, object obj, Type parentType)
        {
            Name = name;
            Obj = obj;
            ParentType = parentType;
        }
    }

    #region Attribute definition
    
    /// <summary>
    /// Defines a Property to be shown as a Setting in the Menu
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
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

    /// <summary>
    /// Defines a Property to be shown as a Menu Item in the Menu
    /// </summary>
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property)]
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

    /// <summary>
    /// Defines an Entrance-Point for the Settings-functions to search
    /// It is only allowed to static objects
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ContainSettingsAttribute : Attribute
    {
        public readonly string Name;
        public readonly bool ShowAsMenuItem;

        /// <summary>
        /// Defines an Entrance-Point for the Settings-functions to search
        /// It is only allowed to static objects
        /// </summary>
        /// <param name="name">The Name which will be shown in the Menu</param>
        /// <param name="showAsMenuItem">Defines wheather this object will be shown as a Menu Item. If the Value is set to false, the objects children are still visible. The DefualtValue is true.</param>
        public ContainSettingsAttribute(string name, bool showAsMenuItem = true)
        {
            this.Name = name;
            this.ShowAsMenuItem = showAsMenuItem;
        }
    }
    #endregion
}
