using System;
using System.Collections;
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
        public static readonly Dictionary<Type, float> heightDictionary = new Dictionary<Type, float>(); //defines the percentage height for each Types control
        private static SettingTemplate _instance; //just to make sure, that the static Draw method won't find itself.

        private SettingTemplate() { }

        public static void Initialize()
        {
            _instance = new SettingTemplate();
            heightDictionary.Add(typeof(string), 0.2f);
            heightDictionary.Add(typeof(bool), 0.2f);
            heightDictionary.Add(typeof(int), 0.2f);
            heightDictionary.Add(typeof(byte), 0.2f);
            heightDictionary.Add(typeof(sbyte), 0.2f);
            heightDictionary.Add(typeof(uint), 0.2f);
            heightDictionary.Add(typeof(float), 0.2f);
            heightDictionary.Add(typeof(double), 0.2f);
        }

        public static object Draw(object oldValue, string name, Rect position)
        {
            if (oldValue.GetType().IsArray)
            {
                object[] values = ((IEnumerable) oldValue).Cast<object>().Select(x => x).ToArray(); //the direct conversation (values = object[] oldValue) is not possible
                float height;
                if (SettingTemplate.heightDictionary.TryGetValue(values[0].GetType(), out height))
                    height *= position.height;
                else
                    height = 20f;

                GUI.BeginGroup(position);
                GUI.Label(new Rect(position.x, position.y, position.width, 20f), name);

                for (int i = 0; i<values.Length;i++)
                {
                    SettingTemplate.Draw(values[i], i.ToString(),
                        new Rect(position.x + 20, 22 + position.y + (height + 2)*i, position.width - 20, height));
                }
                GUI.EndGroup();
            }
            else
            {
                //searcch for functions in this class with the same return parameter Type as the values type
                MethodInfo info = typeof(SettingTemplate).GetMethod("Draw",
                    new Type[] {oldValue.GetType(), typeof(string), typeof(Rect)});
                if (info != null && !info.IsStatic) //no static function so that this function do not call itself
                {
                    object[] parameter = new[] {oldValue, name, position};
                    return info.Invoke(_instance, parameter);
                }
            }
            //if there is no function defined for that Type
            GUI.Label(position, "For the Type " + oldValue.GetType().Name + " is no Control Defined");
            return null;
        }

        public bool Draw(bool oldValue, string name, Rect position)
        {
            GUI.Label(new Rect(position.x, position.y, position.width*0.3f, position.height), name);
            return
                GUI.Toggle(
                    new Rect(position.x + position.width*0.3f, position.y, position.width*0.7f, position.height),
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
            return (sbyte) Draw((int) oldValue, name, position);
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
    }
}
