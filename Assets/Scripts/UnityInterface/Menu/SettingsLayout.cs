using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImportantClasses;
using UnityEngine;

namespace UnityInterface.Menu
{
    class SettingsLayout : MonoBehaviour
    {
        private List<string> _namesList;
        private Vector2 _itemScrollPosition;
        private Vector2 _settingScrollPosition;

        void Start()
        {
            _namesList = new List<string>();
            Settings.Initialize();
        }

        void OnGUI()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal(); //Bar on the top to go back
            if(GUILayout.Button("home"))
                _namesList.Clear();
            for(int i = 0; i < _namesList.Count-1; i++)
            {
                if (GUILayout.Button(_namesList.ElementAt(i)))
                {
                    _namesList = _namesList.GetRange(0, i+1);
                    break;
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            DrawMenuItems();
            DrawSettings();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawMenuItems()
        {
            List<string> menuItems =
                Settings.GetMenuItems(
                    _namesList.ToArray());

            //GUILayout.BeginArea(new Rect(10, 20, Screen.width/5 - 10, Screen.height - 20));
            _itemScrollPosition = GUILayout.BeginScrollView(_itemScrollPosition);
            GUILayout.BeginVertical();
            foreach (string menuItem in menuItems)
            {
                if (GUILayout.Button(menuItem))
                {
                    _namesList.Add(menuItem);
                    break;
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            //GUILayout.EndArea();
        }

        private void DrawSettings()
        {
            Dictionary<string,object> namesDictionary = Settings.GetMenuSettings(_namesList.ToArray());

            GUILayout.BeginVertical();
            _settingScrollPosition = GUILayout.BeginScrollView(_settingScrollPosition);
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }
    }
}
