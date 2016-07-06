using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace UnityInterface.SettingTemplates
{
    /// <summary>
    /// provides Type specific Draw Functions for the GUI and defindes their height
    /// </summary>
    public class SettingTemplate
    {
        private static readonly Dictionary<Type, float> heightDictionary = new Dictionary<Type, float>(); //defines the relative heigth to _inputHeigth for each Types control
        private static SettingTemplate _instance; //just to make sure, that the static Draw method won't find itself, but all the other methods
        private static float _inputHeight = 30;
        private static float _padding = 2;

        private static SettingTemplate Instance
        {
            get
            {
                if(_instance ==null)
                    Initialize();
                return _instance;
            }
        }

        //singleton
        private SettingTemplate() { }

        /// <summary>
        /// set the instance and the height for each Type
        /// </summary>
        private static void Initialize()
        {
            _instance = new SettingTemplate();
            heightDictionary.Add(typeof(string), 1f);
            heightDictionary.Add(typeof(bool), 1f);
            heightDictionary.Add(typeof(int), 1f);
            heightDictionary.Add(typeof(byte), 1f);
            heightDictionary.Add(typeof(sbyte), 1f);
            heightDictionary.Add(typeof(uint), 1f);
            heightDictionary.Add(typeof(float), 1f);
            heightDictionary.Add(typeof(double), 1f);
        }

        /// <summary>
        /// sets the height which is used to calculate the height of all controls
        /// </summary>
        /// <param name="inputHeight">the height of a TextField with a height of a single textline</param>
        public static void SetInputHeight(float inputHeight)
        {
            _inputHeight = inputHeight;
        }

        /// <summary>
        /// sets the padding between two controls of an array
        /// </summary>
        /// <param name="padding">the padding between two controls</param>
        public static void SetPadding(float padding)
        {
            _padding = padding;
        }

        /// <summary>
        /// calculates the relative height of a control of a specific Type. normally its factor 0.2
        /// </summary>
        /// <param name="type">The Type of the value of which the height is asked</param>
        /// <returns>The relative height of the control</returns>
        public static float GetHeight(Type type)
        {
            float height = 0;
            if (heightDictionary.TryGetValue(type, out height))
                return height*_inputHeight;
            else
                return _inputHeight;
        }

        /// <summary>
        /// Draw a control for the given object to be able to edit its value
        /// </summary>
        /// <param name="oldValue">the parameter which shall be shown</param>
        /// <param name="name">the name of the parameter which stands beneath the input field</param>
        /// <param name="position">the position of the control</param>
        /// <returns>the new value of the parameter</returns>
        public static object Draw(object oldValue, string name, Rect position)
        {
            //search for functions in this class with the same return parameter Type as the oldValues type
            //calling Draw directly will not work -> reflection is used
            MethodInfo info = typeof(SettingTemplate).GetMethod("Draw", 
                new Type[] { oldValue.GetType(), typeof(string), typeof(Rect) });
            if (info != null && !info.IsStatic) //no static function so that this function do not call itself
            {
                object[] parameter = new[] { oldValue, name, position };
                return info.Invoke(Instance, parameter); //calling the method
            }

            //if there is no function defined for that Type: show a Text
            GUI.Label(position, "For the type " + oldValue.GetType().Name + " is no control defined");
            return null;
        }

        /// <summary>
        /// Draw a control for the given object array to be able to edit its value
        /// </summary>
        /// <param name="oldValue">the parameter which shall be shown</param>
        /// <param name="name">the name of the parameter which stands beneath the input field</param>
        /// <param name="position">the position of the control</param>
        /// <returns>the new value of the parameter</returns>
        public static object DrawArray(object[] oldValue, string name, Rect position)
        {
            if(oldValue.Length == 0)
                return oldValue;

            float height = GetHeight(oldValue[0].GetType());

            GUI.BeginGroup(position); //Group all the controls
            GUI.Label(new Rect(0, 0, position.width, _inputHeight), name);

            List<object> newValues = new List<object>();//save the values in case that they are changed
            for (int i = 0; i < oldValue.Length; i++)
            {
                object value = Draw(oldValue[i], (i+1).ToString(),
                    new Rect(0, (height + _padding) * (i+1), position.width, height));
                newValues.Add(value);
            }
            GUI.EndGroup();

            if (!ImportantClasses.Helper.ArrayValueEqual(oldValue,newValues.ToArray())) //test if a value were changed
                return newValues.ToArray();
            else
                return oldValue; //the old Value must be returned when it was not changed because otherwise the normal equal function will return the wrong result
        }

 #region Type-specific drawing functions
        //the functions must be public to be found
        //the Type of the oldValue and the return value must be the same

        public bool Draw(bool oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width * 0.3f, position.height), name);
            return
                GUI.Toggle(
                    new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.7f, position.height),
                    oldValue, name);
        }

        public string Draw(string oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width * 0.3f, position.height), name);
            return GUI.TextField(new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.7f, position.height), oldValue);
        }

        public int Draw(int oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width * 0.3f, position.height), name);
            int val;
            string s = GUI.TextField(new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.7f, position.height), oldValue.ToString());
            if (int.TryParse(s, out val))
                return val;
            else
                return oldValue;
        }

        public sbyte Draw(sbyte oldValue, string name, Rect position)
        {
            return (sbyte)Draw((int)oldValue, name, position);
        }

        public byte Draw(byte oldValue, string name, Rect position)
        {
            return (byte)Draw((int)oldValue, name, position);
        }

        public float Draw(float oldValue, string name, Rect position)
        {
            return (float)Draw((double)oldValue, name, position);
        }

        public double Draw(double oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width * 0.3f, position.height), name);
            double val;
            string s = GUI.TextField(new Rect(position.x + position.width * 0.3f, position.y, position.width * 0.7f, position.height), oldValue.ToString("N3"));
            if (double.TryParse(s, out val))
                return val;
            else
                return oldValue;
        }
 #endregion
    }
}
