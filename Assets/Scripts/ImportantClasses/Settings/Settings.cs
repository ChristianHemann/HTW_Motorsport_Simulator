using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using UnityEditor;

namespace ImportantClasses
{
    /// <summary>
    /// Saves the Settings, read them when the Software is started and Provides Functions to show them in the GUI
    /// Before using this class the Initialize()-Function must be called
    /// </summary>
    public static class Settings
    {
        private static bool _isInitialized = false;
        private static readonly List<ContainSettingObject> settingList = new List<ContainSettingObject>();
        private static string settingsPath = @"D:\Studium\SWE\";

        /// <summary>
        /// Initialize the class. Without Initialization the class cannot work.
        /// </summary>
        public static void Initialize()
        {
            if (!_isInitialized)
            {
                SearchForContainSettingsAttribute();
                _isInitialized = true;
            }
        }

        /// <summary>
        /// generates a list of names which are the Menu Items of the specified parent.
        /// </summary>
        /// <param name="names">specifies the parents as a hierachy. If an empty array is passed, the root level will be choosen. E.g. names[0] could be "Car".</param>
        /// <returns>the List of Menu Items</returns>
        public static List<string> GetMenuItems(string[] names)
        {
            List<string> children = new List<string>();
            object parent = null;
            if (0 == names.Length) //root; return the Names of the ContainSettingsObjects
            {
                foreach (ContainSettingObject obj in settingList)
                {
                    children.Add(obj.Name);
                }
                return children;
            }

            foreach (ContainSettingObject obj in settingList) //find the first child
            {
                if (names[0] == obj.Name)
                {
                    parent = obj.Obj;
                    break;
                }
            }

            for (int i = 0; i < names.Length; i++)
            {
                if (parent == null) //this should not happen
                    return children;

                IEnumerable<FieldInfo> infos = parent.GetType()
                    .GetFields()
                    .Where(prop => prop.IsDefined(typeof(SettingMenuItemAttribute), false));
                if (i == names.Length - 1) //when the destinated level is reached
                {
                    foreach (FieldInfo info in infos)
                    {
                        children.Add(((SettingMenuItemAttribute)
                            info.GetCustomAttributes(typeof(SettingMenuItemAttribute), false).First()).Name);
                    }
                    return children;
                }
                else
                {
                    foreach (FieldInfo info in infos)
                    {
                        if (((SettingMenuItemAttribute)
                            info.GetCustomAttributes(typeof(SettingMenuItemAttribute), false).First()).Name == names[i])
                        {
                            parent = info.GetValue(parent); //go one level deeper
                            break;
                        }
                    }
                }
            }


            return null;
        }

        public static List<object> GetMenuSettings(string[] names)
        {
            
        }

        public static void WriteSettingsFromMenu()
        {
            
        }

        /// <summary>
        /// saves all current setting to the file which is used last for each object. If no file is specified for an object a saveFilePanel will appear
        /// </summary>
        public static void SaveAllSettings()
        {
            foreach (ContainSettingObject obj in settingList)
            {
                SaveSetting(obj.Name, obj.Path);
            }
        }

        /// <summary>
        /// saves the current setting of the specified object
        /// </summary>
        /// <param name="name">the name which is specified in the ContainSettingsAttribute of the object which shall be saved</param>
        /// <param name="path">The path to the file. If empty or "" a saveFilePanel will appear</param>
        public static void SaveSetting(string name, string path = "")
        {
            try
            {
                if (path.Length == 0)
                {
                    path = EditorUtility.SaveFilePanel("Save Setting "+name, settingsPath, name + ".xml", "xml");
                    if (path.Length == 0) //no file selected
                        return;
                }
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(path);
                foreach (ContainSettingObject obj in settingList)
                {
                    if (name == obj.Name)
                    {
                        Xml.WriteXml(path, obj.Obj);
                        obj.Path = path;
                        return;
                    }
                }
            }
            catch
            {
                EditorUtility.DisplayDialog("error while saving", "An error occured during saving the file", "Ok");
            }
        }

        /// <summary>
        /// Load all objects which are marked as a setting from a file. If no path for a file was specified an openFilePanel will appear
        /// </summary>
        public static void LoadAllSettings()
        {
            foreach (ContainSettingObject obj in settingList)
            {
                LoadSettings(obj.Name, obj.Path);
            }
        }

        /// <summary>
        /// Load an object which is marked with the ContainSettingsAttribute
        /// </summary>
        /// <param name="name">The Name which is defined in the ContainSettingsAttribute of the object</param>
        /// <param name="path">If the path of the file is allready known it can be placed here. If empty or "" an OpenFilePanel will appear</param>
        public static void LoadSettings(string name, string path = "")
        {
            if (path.Length == 0)
            {
                path = EditorUtility.OpenFilePanel("Open File " + name, settingsPath, "xml");
                if (path.Length == 0) //if no file was specified
                    return;
            }
            if (!Directory.Exists(Path.GetDirectoryName(path))) //If a wrong path was selected or the path were deleted 
            {
                EditorUtility.DisplayDialog("File not found", "The specified file could not be found. Path: " + path, "Ok");
                return;
            }

            foreach (ContainSettingObject actObj in settingList)
            {
                if(actObj.Name != name) //Find the correct object
                    continue;
                try
                {
                    //search in fields
                    FieldInfo[] fieldInfos = actObj.ParentType.GetFields();
                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        //relevant when one class contains more than one object marked with ContainSettingsAttribute
                        if (((ContainSettingsAttribute) Attribute.GetCustomAttribute(fieldInfo, typeof(ContainSettingsAttribute))).Name == actObj.Name)
                        {

                            object obj = Xml.ReadXml(Path.Combine(settingsPath, actObj.Name + ".xml"),
                                actObj.Obj.GetType());
                            //update the reference to the object in the list and in the original class
                            fieldInfo.SetValue(null, obj);
                            actObj.Obj = obj;
                        }
                    }
                    //search in properties
                    PropertyInfo[] propertyInfos = actObj.ParentType.GetProperties();
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        if (((ContainSettingsAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ContainSettingsAttribute))).Name == actObj.Name)
                        {
                            object obj = Xml.ReadXml(Path.Combine(settingsPath, actObj.Name + ".xml"), actObj.Obj.GetType());
                            //update the reference to the object in the list and in the original class
                            propertyInfo.SetValue(null, obj, null);
                            actObj.Obj = obj;
                        }
                    }
                }
                catch
                {
                    EditorUtility.DisplayDialog("error while loading File " + name,
                        "The File " + name + " could not be loaded. Possibly the file do not contain an object of the correct Type",
                        "Ok");
                }

            }
        }

        /// <summary>
        /// fills the settingList with all the objects which has the ContainSettingsAttribute
        /// </summary>
        private static void SearchForContainSettingsAttribute()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    //Add all fields to the List
                    IEnumerable<FieldInfo> fieldInfos = type.GetFields().Where(prop => prop.IsDefined(typeof(ContainSettingsAttribute), false));
                    foreach (FieldInfo info in fieldInfos)
                    {
                        ContainSettingsAttribute[] attrs = ((ContainSettingsAttribute[])
                                info.GetCustomAttributes(typeof(ContainSettingsAttribute), false));
                        if (attrs.Length != 1)
                            throw new IndexOutOfRangeException("The field " + info.Name + " has more than one attribute of the Type ContainSettingsAttribute");
                        if (info.IsStatic)
                            settingList.Add(new ContainSettingObject(attrs.First().Name, info.GetValue(null), type, Path.Combine(settingsPath,attrs.First().Name+".xml")));
                    }

                    //Add all Properties to the List
                    IEnumerable<PropertyInfo> propertyInfos = type.GetProperties().Where(prop => prop.IsDefined(typeof(ContainSettingsAttribute), false));
                    foreach (PropertyInfo info in propertyInfos)
                    {
                        ContainSettingsAttribute[] attrs = ((ContainSettingsAttribute[])
                                info.GetCustomAttributes(typeof(ContainSettingsAttribute), false));
                        if (attrs.Length != 1)
                            throw new IndexOutOfRangeException("The property " + info.Name + " has more than one attribute of the Type ContainSettingsAttribute");
                        if (info.GetAccessors().First().IsStatic)
                            settingList.Add(new ContainSettingObject(attrs.First().Name, info.GetValue(null, null), type, Path.Combine(settingsPath, attrs.First().Name + ".xml")));
                    }
                }
            }
        }
    }
}
