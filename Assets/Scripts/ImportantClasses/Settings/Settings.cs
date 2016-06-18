using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ImportantClasses
{
    /// <summary>
    /// Saves the Settings, read them when the Software is started and Provides Functions to show them in the GUI
    /// Before using this class the Initialize()-Function must be called
    /// 
    /// For informations about the attributes to use see the classes ContainSettingsAttribute, SettingMenuItemAttribute and SettingAttribute
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// The Default Path to store Settings or other Files of which the Path should not change
        /// </summary>
        public static string SettingsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_settingsPath))
                    Initialize();
                return _settingsPath;
            }
        }
        private static bool _isInitialized = false; //says if the Method Initialize was called
        private static readonly List<ContainSettingObject> SettingList = new List<ContainSettingObject>(); //Contains all objects with the ContainSettingsAttribute
        private static string _settingsPath = ""; //The Default Settings Path
        private static Dictionary<string[], object> _changeBufferDictionary;

        /// <summary>
        /// Initialize the class. Without Initialization the class cannot work.
        /// </summary>
        public static void Initialize()
        {
            if (!_isInitialized)
            {
                //Path in Windows: C:\Programm Data\Simulator
                _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Simulator");
                if (!Directory.Exists(_settingsPath))
                    Directory.CreateDirectory(_settingsPath);
                _changeBufferDictionary = new Dictionary<string[], object>();
                SearchForContainSettingsAttribute();
                SearchForSettingMenuItemAttribute();
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
            Dictionary<string, object> actDictionary = null;
            if (names.Length == 0) //if no name: get all objects out of SettingList; create the Dictionary
            {
                actDictionary = new Dictionary<string, object>();
                foreach (ContainSettingObject containSettingObject in SettingList)
                {
                    actDictionary.Add(containSettingObject.Name, containSettingObject.NamesHierachy);
                }
            }
            else
            {
                foreach (ContainSettingObject containSettingObject in SettingList)
                {
                    if (containSettingObject.Name == names.First())
                    {
                        actDictionary = containSettingObject.NamesHierachy;
                        break;
                    }
                }
            }
            if (actDictionary == null)
                return null;

            for (int i = 1; i < names.Length; i++) //go to the destinated dictionary
            {
                object obj;
                if (actDictionary.TryGetValue(names[i], out obj))
                {
                    if (obj is Dictionary<string, object>)
                        actDictionary = (Dictionary<string, object>)obj;
                    else
                        return null; //the object is not a menuItem
                }
                else
                {
                    return null; //no object was found
                }
            }
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, object> keyValuePair in actDictionary)
            {
                if (keyValuePair.Value is Dictionary<string, object>) //add just if it is a menuItem
                    list.Add(keyValuePair.Key);
            }
            return list;
        }

        /// <summary>
        /// searches for all Settings in the given Path
        /// </summary>
        /// <param name="names">the path to search for settings. each entry in the array represents the next level. i.e: names[0] = car, names[1] = suspension</param>
        /// <returns>return the values with the names</returns>
        public static Dictionary<string, object> GetMenuSettings(string[] names)
        {
            //NamesHierachy cannot be used, because of problems in getting the right Instance
            Dictionary<string, object> settingsDictionary = new Dictionary<string, object>();

            if (names.Length == 0) //if no name there cannot be any setting
                return settingsDictionary;

            //int i = 0; //find when to stop the iteration of the parents ans get the settings
            foreach (ContainSettingObject obj in SettingList) //go to the correct parent in settingsList
            {
                if (obj.Name == names.First())
                {
                    object parent = obj.Obj;
                    for (int i = 1; i <= names.Length; i++)
                    //foreach (string name in names)
                    {
                        if (i == names.Length) //get settings
                        {
                            IEnumerable<FieldInfo> fieldInfos =
                                parent.GetType()
                                    .GetFields()
                                    .Where(field => field.IsDefined(typeof(SettingAttribute), false));
                            foreach (FieldInfo fieldInfo in fieldInfos)
                            {
                                string settingName = ((SettingAttribute)
                                    fieldInfo.GetCustomAttributes(typeof(SettingAttribute), false).First()).Name;
                                List<string> changeName = names.ToList(); //here the name (key) of the changed Control will be generated for comparision
                                changeName.Add(settingName);
                                string[] changeNameArray = changeName.ToArray(); //Convert the List into an Array
                                bool containsKey = false; //saves whether the key was found
                                foreach (KeyValuePair<string[], object> valuePair in _changeBufferDictionary) //If the value was changed: show the changed value
                                {
                                    if (Helper.ArrayValueEqual<string>(valuePair.Key, changeNameArray)) //The Method _changeBufferDictionary.ContainsKey() is not suitable, because it return false if the array is not the same but has the same content
                                    {
                                        containsKey = true;
                                        //If the value was changed: show the changed value
                                        object value;
                                        _changeBufferDictionary.TryGetValue(valuePair.Key, out value);
                                        settingsDictionary.Add(settingName, value);
                                        break;
                                    }
                                }
                                if (!containsKey) //add just if it was not changed
                                    settingsDictionary.Add(settingName, fieldInfo.GetValue(parent));
                            }
                            IEnumerable<PropertyInfo> propertyInfos =
                                parent.GetType()
                                    .GetProperties()
                                    .Where(prop => prop.IsDefined(typeof(SettingAttribute), false));
                            foreach (PropertyInfo propertyInfo in propertyInfos)
                            {
                                string settingName = ((SettingAttribute)
                                    propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).First()).Name;
                                List<string> changeName = names.ToList(); //here the name (key) of the changed Control will be generated for comparision
                                changeName.Add(settingName);
                                string[] changeNameArray = changeName.ToArray(); //Convert the List into an Array
                                bool containsKey = false; //saves whether the key was found
                                foreach (KeyValuePair<string[], object> valuePair in _changeBufferDictionary) //If the value was changed: show the changed value
                                {
                                    if (Helper.ArrayValueEqual<string>(valuePair.Key, changeNameArray)) //The Method _changeBufferDictionary.ContainsKey() is not suitable, because it return false if the array is not the same but has the same content
                                    {
                                        containsKey = true;
                                        //If the value was changed: show the changed value
                                        object value;
                                        _changeBufferDictionary.TryGetValue(valuePair.Key, out value);
                                        settingsDictionary.Add(settingName, value);
                                        break;
                                    }
                                }
                                if (!containsKey) //add just if it was not changed
                                    settingsDictionary.Add(settingName, propertyInfo.GetValue(parent, null));
                            }
                            return settingsDictionary;
                        }
                        else //iterate through parents
                        {
                            bool parentFound = false;
                            IEnumerable<FieldInfo> fieldInfos =
                                parent.GetType().GetFields().Where(field => field.IsDefined(typeof(SettingMenuItemAttribute), false));
                            foreach (FieldInfo fieldInfo in fieldInfos)
                            {
                                if (((SettingMenuItemAttribute)
                                        fieldInfo.GetCustomAttributes(typeof(SettingMenuItemAttribute), false).First()).Name == names[i])
                                {
                                    parent = fieldInfo.GetValue(parent);
                                    parentFound = true;
                                    break;
                                }
                            }
                            if (!parentFound)
                            {
                                IEnumerable<PropertyInfo> propertyInfos =
                                    parent.GetType()
                                        .GetProperties()
                                        .Where(prop => prop.IsDefined(typeof(SettingMenuItemAttribute), false));
                                foreach (PropertyInfo propertyInfo in propertyInfos)
                                {
                                    if (
                                        ((SettingMenuItemAttribute)
                                            propertyInfo.GetCustomAttributes(typeof(SettingMenuItemAttribute), false)
                                                .First()).Name == names[i])
                                    {
                                        parent = propertyInfo.GetValue(parent, null);
                                    }
                                }
                            }
                        }
                    }
                    break;
                }
            }
            return settingsDictionary;
        }

        /// <summary>
        /// change the value of a setting in a temporary buffer; it is adopt when the function SaveTemporaryChanges() is called
        /// </summary>
        /// <param name="names">the name and path to the object which was changed</param>
        /// <param name="value">the value of the changed object</param>
        public static void ChangeSettingTomporary(string[] names, object value)
        {
            string[] key = null;
            foreach (KeyValuePair<string[], object> keyValuePair in _changeBufferDictionary)
            {
                if (Helper.ArrayValueEqual<string>(keyValuePair.Key, names)) //The Method _changeBufferDictionary.ContainsKey() is not suitable, because it return false if the array is not the same but has the same content
                {
                    key = keyValuePair.Key;
                    break;
                }
                //bool allValuesFound = true;
                //for (int i = 0; i < keyValuePair.Key.Length && i < names.Length; ++i)
                //{
                //    if (keyValuePair.Key[i] != names[i])
                //    {
                //        allValuesFound = false;
                //        break;
                //    }
                //}
                //if (allValuesFound)
                //    key = keyValuePair.Key;
            }
            if (key != null)
                _changeBufferDictionary.Remove(key);

            _changeBufferDictionary.Add(names, value);
        }

        /// <summary>
        /// Saves all Settings which were changed temporary
        /// </summary>
        /// <param name="only">the name of the containSettingsAttribute which shall be saved</param>
        public static void SaveTemporaryChanges(string only = "")
        {
            if (_changeBufferDictionary.Count == 0)
                return;
            foreach (KeyValuePair<string[], object> keyValuePair in _changeBufferDictionary)
            {
                if (only.Length != 0 && keyValuePair.Key.First() != only)
                    continue;

                object parent = null;
                foreach (ContainSettingObject containSettingObject in SettingList)
                {
                    if (containSettingObject.Name == keyValuePair.Key.First())
                    {
                        parent = containSettingObject.Obj;
                        break;
                    }
                }
                if (parent == null)
                    return;
                for (int i = 1; i < keyValuePair.Key.Length - 1; i++)
                {
                    bool parentFound = false;
                    IEnumerable<FieldInfo> fieldInfos =
                        parent.GetType().GetFields().Where(prop => prop.IsDefined(typeof(SettingMenuItemAttribute), false));
                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        SettingMenuItemAttribute attr =
                            (SettingMenuItemAttribute)
                                fieldInfo.GetCustomAttributes(typeof(SettingMenuItemAttribute), false).First();
                        if (attr.Name == keyValuePair.Key[i])
                        {
                            parent = fieldInfo.GetValue(parent);
                            parentFound = true;
                            break;
                        }
                    }
                    if (!parentFound)
                    {
                        IEnumerable<PropertyInfo> propertyInfos =
                            parent.GetType()
                                .GetProperties()
                                .Where(prop => prop.IsDefined(typeof(SettingMenuItemAttribute), false));
                        foreach (PropertyInfo propertyInfo in propertyInfos)
                        {
                            SettingMenuItemAttribute attr =
                                (SettingMenuItemAttribute)
                                    propertyInfo.GetCustomAttributes(typeof(SettingMenuItemAttribute), false).First();
                            if (attr.Name == keyValuePair.Key[i])
                            {
                                parent = propertyInfo.GetValue(parent, null);
                                break;
                            }
                        }
                    }
                }
                //parent found; get value

                bool valueSet = false;
                IEnumerable<FieldInfo> fieldInfos2 = parent.GetType().GetFields().Where(prop => prop.IsDefined(typeof(SettingAttribute), false));
                foreach (FieldInfo fieldInfo in fieldInfos2)
                {
                    SettingAttribute attr = (SettingAttribute)fieldInfo.GetCustomAttributes(typeof(SettingAttribute), false).First();
                    if (attr.Name == keyValuePair.Key.Last())
                    {
                        object value;
                        if (keyValuePair.Value.GetType().IsArray)
                        {
                            object[] values = ((IEnumerable)keyValuePair.Value).Cast<object>().ToArray(); //cannot cast directly from object to object[]
                            //convert object[] to an array of the correct type to avoid contravariance
                            System.Type type = fieldInfo.FieldType.GetElementType();
                            Array typeSpecificArray = Array.CreateInstance(type, values.Length);
                            Array.Copy(values, typeSpecificArray, values.Length);
                            value = typeSpecificArray;
                        }
                        else
                            value = keyValuePair.Value;

                        fieldInfo.SetValue(parent, value);
                        valueSet = true;
                        break;
                    }
                }
                if (!valueSet)
                {
                    IEnumerable<PropertyInfo> propertyInfos2 =
                        parent.GetType().GetProperties().Where(prop => prop.IsDefined(typeof(SettingAttribute), false));
                    foreach (PropertyInfo propertyInfo in propertyInfos2)
                    {
                        //propertyInfo.PropertyType
                        object value;
                        if (keyValuePair.Value.GetType().IsArray)
                        {
                            object[] values = ((IEnumerable)keyValuePair.Value).Cast<object>().ToArray(); //cannot cast directly from object to object[]
                            //convert object[] to an array of the correct type to avoid contravariance
                            System.Type type = propertyInfo.PropertyType.GetElementType();
                            Array typeSpecificArray = Array.CreateInstance(type, values.Length);
                            Array.Copy(values, typeSpecificArray, values.Length);
                            value = typeSpecificArray;
                        }
                        else
                            value = keyValuePair.Value;

                        SettingAttribute attr =
                            (SettingAttribute)propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).First();
                        if (attr.Name == keyValuePair.Key.Last())
                        {
                            propertyInfo.SetValue(parent, value, null);
                            break;
                        }
                    }
                }
            }
            _changeBufferDictionary.Clear();
        }

        /// <summary>
        /// discards the Changes in the Settings
        /// </summary>
        public static void DiscardTemporaryChanges()
        {
            _changeBufferDictionary.Clear();
        }

        /// <summary>
        /// says if there are unsaved temporary changes
        /// </summary>
        /// <returns></returns>
        public static bool HasTemporaryChanges()
        {
            return _changeBufferDictionary.Count != 0;
        }

        /// <summary>
        /// saves all current setting to the file which is used last for each object. If no file is specified for an object the object will not be saved
        /// </summary>
        public static void SaveAllSettings()
        {
            foreach (ContainSettingObject obj in SettingList)
            {
                SaveSetting(obj.Name, obj.Path);
            }
        }

        /// <summary>
        /// saves the current setting of the specified object
        /// </summary>
        /// <param name="name">the name which is specified in the ContainSettingsAttribute of the object which shall be saved</param>
        /// <param name="path">The path to the file</param>
        public static void SaveSetting(string name, string path)
        {
            try
            {
                if (String.IsNullOrEmpty(path))
                {
                    Message.Send("There is no valid file selected for saving " + name, Message.MessageCode.Warning);
                }
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(path);
                foreach (ContainSettingObject obj in SettingList)
                {
                    if (name == obj.Name)
                    {
                        Xml.WriteXml(path, obj.Obj);
                        obj.Path = path;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Message.Send("An error occured during saving the file", Message.MessageCode.Error);
            }
        }

        /// <summary>
        /// Load all objects which are marked as a setting from a file. If no path for a file was specified an openFilePanel will appear
        /// </summary>
        public static void LoadAllSettings()
        {
            foreach (ContainSettingObject obj in SettingList)
            {
                LoadSettings(obj.Name, obj.Path);
            }
        }

        /// <summary>
        /// Load an object which is marked with the ContainSettingsAttribute
        /// </summary>
        /// <param name="name">The Name which is defined in the ContainSettingsAttribute of the object</param>
        /// <param name="path">The path to the file to be loaded</param>
        public static void LoadSettings(string name, string path)
        {
            if (String.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Message.Send("There is no valid File selected to open " + name, Message.MessageCode.Warning);
                return;
            }

            foreach (ContainSettingObject actObj in SettingList)
            {
                if (actObj.Name != name) //Find the correct object
                    continue;
                //try
                //{
                    //search in fields
                    IEnumerable<FieldInfo> fieldInfos =
                        actObj.ParentType.GetFields()
                            .Where(field => field.IsDefined(typeof(ContainSettingsAttribute), false));
                    foreach (FieldInfo fieldInfo in fieldInfos)
                    {
                        //relevant when one class contains more than one object marked with ContainSettingsAttribute
                        if (((ContainSettingsAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(ContainSettingsAttribute))).Name == actObj.Name)
                        {
                            object obj = Xml.ReadXml(path, actObj.Obj.GetType());
                            //update the reference to the object in the list and in the original class
                            fieldInfo.SetValue(null, obj);
                            actObj.Obj = obj;
                            actObj.Path = path;
                        }
                    }
                    //search in properties
                    IEnumerable<PropertyInfo> propertyInfos =
                        actObj.ParentType.GetProperties()
                            .Where(prop => prop.IsDefined(typeof(ContainSettingsAttribute), false));
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        if (((ContainSettingsAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ContainSettingsAttribute))).Name == actObj.Name)
                        {
                            object obj = Xml.ReadXml(path, actObj.Obj.GetType());
                            //update the reference to the object in the list and in the original class
                            propertyInfo.SetValue(null, obj, null);
                            actObj.Obj = obj;
                            actObj.Path = path;
                        }
                    }
                //}
                //catch (Exception ex)
                //{
                //    Message.Send("The File " + name + " could not be loaded. Possibly the file do not contain an object of the correct Type", Message.MessageCode.Warning);
                //    throw new Exception("could not load the file", ex);
                //}

            }
        }

        /// <summary>
        /// gets the SettingAttribute of a property or field
        /// </summary>
        /// <param name="parentClass">the Type of the class which contains the object with the attribute</param>
        /// <param name="attributeName">the name which is defined in the Attribute definition</param>
        /// <returns>The SettingAttribute of the object. null if the atttribute was not found</returns>
        public static SettingAttribute GetAttribute(Type parentClass, string attributeName)
        {
            IEnumerable<FieldInfo> fieldInfos =
                parentClass.GetFields().Where(field => field.IsDefined(typeof(SettingAttribute), false));
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                SettingAttribute attr =
                    (SettingAttribute)fieldInfo.GetCustomAttributes(typeof(SettingAttribute), false).First();
                if (attr.Name == attributeName)
                    return attr;
            }

            IEnumerable<PropertyInfo> propertyInfos =
                parentClass.GetProperties().Where(prop => prop.IsDefined(typeof(SettingAttribute), false));
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                SettingAttribute attr =
                    (SettingAttribute)propertyInfo.GetCustomAttributes(typeof(SettingAttribute), false).First();
                if (attr.Name == attributeName)
                    return attr;
            }
            return null;
        }

        /// <summary>
        /// fills the SettingList with all the objects which has the ContainSettingsAttribute
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
                            SettingList.Add(new ContainSettingObject(attrs.First().Name, info.GetValue(null), type, Path.Combine(_settingsPath, attrs.First().Name + ".xml")));
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
                            SettingList.Add(new ContainSettingObject(attrs.First().Name, info.GetValue(null, null), type, Path.Combine(_settingsPath, attrs.First().Name + ".xml")));
                    }
                }
            }
        }

        /// <summary>
        /// Search in the List of all ContainSettingAttributes for all Attributes with the SettingMenuItemAttribute an SettingAttribute
        /// </summary>
        private static void SearchForSettingMenuItemAttribute()
        {
            foreach (ContainSettingObject obj in SettingList)
            {
                SearchForSettingMenuItemAttribute(obj.Obj, obj.NamesHierachy);
            }
        }

        /// <summary>
        /// Search for all Attributes with the SettingMenuItemAttribute and SettingAttribute at the given object
        /// </summary>
        /// <param name="parent">the parent object to search for attributes</param>
        /// <param name="parentDictionary">the dictionary to add the found attributes</param>
        private static void SearchForSettingMenuItemAttribute(object parent, Dictionary<string, object> parentDictionary)
        {
            IEnumerable<FieldInfo> fieldInfos = parent.GetType().GetFields().Where(prop => prop.IsDefined(typeof(SettingMenuItemAttribute), false));
            foreach (FieldInfo fieldInfo in fieldInfos)
            {
                Dictionary<string, object> childDictionary = new Dictionary<string, object>();
                SettingMenuItemAttribute[] attrs =
                    (SettingMenuItemAttribute[])fieldInfo.GetCustomAttributes(typeof(SettingMenuItemAttribute), false);
                parentDictionary.Add(attrs.First().Name, childDictionary);
                SearchForSettingMenuItemAttribute(fieldInfo.GetValue(parent), childDictionary);
            }

            IEnumerable<PropertyInfo> propertyInfos =
                parent.GetType().GetProperties().Where(prop => prop.IsDefined(typeof(SettingMenuItemAttribute), false));
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                Dictionary<string, object> childDictionary = new Dictionary<string, object>();
                SettingMenuItemAttribute[] attrs =
                    (SettingMenuItemAttribute[])
                        propertyInfo.GetCustomAttributes(typeof(SettingMenuItemAttribute), false);
                parentDictionary.Add(attrs.First().Name, childDictionary);
                SearchForSettingMenuItemAttribute(propertyInfo.GetValue(parent, null), childDictionary);
            }
        }
    }
}

