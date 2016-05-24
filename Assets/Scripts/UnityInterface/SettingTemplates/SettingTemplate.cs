using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace UnityInterface.SettingTemplates
{
    public class SettingTemplate
    {
        public static readonly Dictionary<Type, float> heightDictionary = new Dictionary<Type, float>();
        private static SettingTemplate _instance;

        private SettingTemplate() { }

        public static void Initialize()
        {
            _instance = new SettingTemplate();
            heightDictionary.Add(typeof(string), 0.2f);
            heightDictionary.Add(typeof(bool), 0.2f);
        }

        public static object Draw(object oldValue, string name, float height, float width)
        {
            //searcch for functions in this class with the same return parameter Type as the values type
            MethodInfo info = typeof(SettingTemplate).GetMethod("Draw",
                new Type[] { oldValue.GetType(), typeof(string), typeof(float), typeof(float) });
            if (info != null)
            {
                object[] parameter = new[] {oldValue, name, height, width};
                return info.Invoke(_instance, parameter);
            }

            //if there is no function defined for that Type
            GUI.TextField(new Rect(0, 0, width, height), "For the Type " + oldValue.GetType().Name + " is no Control Defined");
            return null;
        }

        public bool Draw(bool oldValue, string name, float height, float width)
        {
            return GUI.Toggle(new Rect(0, 0, width, height), oldValue, name);
        }

        public string Draw(string oldValue, string name, float height, float width)
        {
            return GUI.TextField(new Rect(0, 0, width, height), oldValue);
        }

        public int Draw(int oldValue, string name, float height, float width)
        {
            int val;
            string s = GUI.TextField(new Rect(0, 0, width, height), oldValue.ToString());
            if (int.TryParse(s, out val))
                return val;
            else
                return oldValue;
        }

        public float Draw(float oldValue, string name, float height, float width)
        {
            return (float)Draw((double)oldValue, name, height, width);
        }

        public double Draw(double oldValue, string name, float height, float width)
        {
            double val;
            string s = GUI.TextField(new Rect(0, 0, width, height), oldValue.ToString());
            if (double.TryParse(s, out val))
                return val;
            else
                return oldValue;
        }
    }
}
