using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityInterface.SettingTemplates;
using ImportantClasses;
using UnityEngine;

namespace UnityInterface
{
    class Menu : MonoBehaviour
    {
        private List<string> _namesList;
        private Vector2 _itemScrollPosition;
        private Vector2 _settingScrollPosition;
        private bool showSettings = false;

        //Define Size
        private float padding;
        private float contentWidth;
        private float topBarHeight;
        private float topBarButtonWidth;
        private float buttonHeight;
        private float contentHeight;
        private float menuItemWidth;
        private float menuSettingWidth;

        void Start()
        {
            _namesList = new List<string>();
            Settings.Initialize();
            SettingTemplate.Initialize();
            CalculateSize();
        }

        void OnGUI()
        {
            if (!showSettings)
                DrawMainMenu();
            else //Draw Settings
            {
                GUI.BeginGroup(new Rect(padding, padding, contentWidth, contentHeight)); //padding
                GUI.BeginGroup(new Rect(0, 0, contentWidth, topBarHeight)); //Bar on the top
                DrawTopBar();
                GUI.EndGroup();

                GUI.BeginGroup(new Rect(0, topBarHeight, menuItemWidth, contentHeight - topBarHeight)); //MenuItems
                //GUI.BeginScrollView(); //Read the Documentation
                DrawMenuItems();
                //GUI.EndScrollView();
                GUI.EndGroup(); //End MenuItems

                GUI.BeginGroup(new Rect(menuItemWidth + padding, topBarHeight, menuSettingWidth,
                    contentHeight - topBarHeight)); //Settings
                //GUI.BeginScrollView();
                DrawSettings();
                //GUI.EndScrollView();
                GUI.EndGroup(); //End Settings

                GUI.EndGroup(); //End Content
            }
        }

        private void CalculateSize()
        {
            padding = (Screen.width + Screen.height) / 200f; //1% padding to the edge of the screen
            contentWidth = Screen.width - (2 * padding); //width of the Screeen minus the padding
            contentHeight = Screen.height - (2 * padding); //height of the Screen minus the padding
            topBarHeight = Screen.height / 100 * 10; //10% Height for the Bar on the Top
            buttonHeight = topBarHeight - padding;
            topBarButtonWidth = contentWidth / 8;
            menuItemWidth = contentWidth / 5; //20% of the width for the MenuItems;
            menuSettingWidth = contentWidth - menuItemWidth - padding; //80% minus the padding for the Settings
        }

        private void DrawMainMenu()
        {
            if (GUI.Button(new Rect(menuItemWidth, topBarHeight, contentWidth - 2*menuItemWidth, buttonHeight),
                "Start Race"))
            {
                //load Scene
            }
            if (
                GUI.Button(
                    new Rect(menuItemWidth, topBarHeight + buttonHeight + padding, contentWidth - 2*menuItemWidth,
                        buttonHeight), "Settings"))
            {
                showSettings = true;
            }
        }

        private void DrawTopBar()
        {
            if (GUI.Button(new Rect(0, 0, topBarButtonWidth, buttonHeight), "main"))
            {
                _namesList.Clear();
                showSettings = false;
            }

            if(GUI.Button(new Rect(topBarButtonWidth + padding, 0,topBarButtonWidth, buttonHeight),"settings"))
                _namesList.Clear();

            for (int i = 0; i < _namesList.Count - 1; i++)
            {
                if(GUI.Button(new Rect((i+2)*(topBarButtonWidth+padding), 0, topBarButtonWidth, buttonHeight),_namesList.ElementAt(i)))
                {
                    _namesList = _namesList.GetRange(0, i + 1);
                    break;
                }
            }
        }

        private void DrawMenuItems()
        {
            List<string> menuItems =
                Settings.GetMenuItems(
                    _namesList.ToArray());

            int i = 0;
            foreach (string menuItem in menuItems)
            {
                if (GUI.Button(new Rect(0, i++ * (topBarHeight + padding), menuItemWidth, buttonHeight), menuItem))
                {
                    _namesList.Add(menuItem);
                    break;
                }
            }
        }

        private void DrawSettings()
        {
            Dictionary<string,object> namesDictionary = Settings.GetMenuSettings(_namesList.ToArray());
            float posY = 0;
            foreach (KeyValuePair<string, object> keyValuePair in namesDictionary)
            {
                float height ;
                if(SettingTemplate.heightDictionary.TryGetValue(keyValuePair.Value.GetType(), out height))
                    height *= contentHeight;
                else
                    height = 20f;
                object newValue = SettingTemplate.Draw(keyValuePair.Value, keyValuePair.Key,
                    new Rect(0, posY, menuSettingWidth, height));
                //object newValue = SettingTemplate.Draw(keyValuePair.Value, keyValuePair.Key, height, menuSettingWidth);
                if (newValue != null && !newValue.Equals(keyValuePair.Value)) //if the value was changed: store it temporary
                {
                    List<string> tempList = new List<string>(_namesList); //this list is to add a Value temporary
                    tempList.Add(keyValuePair.Key);
                    Settings.ChangeSettingTomporary(tempList.ToArray(), newValue);
                }
                posY += height + padding/2;
            }
            if (GUI.Button(new Rect(0, posY, menuSettingWidth * 0.24f, contentHeight * 0.2f), "Discard changes"))
                Settings.DiscardTemporaryChanges();

            if (GUI.Button(new Rect(menuSettingWidth*0.25f, posY, menuSettingWidth*0.25f, contentHeight*0.2f),
                "overwrite Settings"))
            {
                Settings.SaveTemporaryChanges();
                Settings.SaveAllSettings();
            }

            if (_namesList.Count != 0) //Show just the selected parent MenuItem
            {
                if (GUI.Button(new Rect(menuSettingWidth*0.51f, posY, menuSettingWidth*0.24f, contentHeight*0.2f),
                    "Save "+_namesList.First()))
                {
                    Settings.SaveTemporaryChanges(_namesList.First());
                    Settings.SaveSetting(_namesList.First());
                }

                if (GUI.Button(new Rect(menuSettingWidth*0.76f, posY, menuSettingWidth*0.24f, contentHeight*0.2f),
                    "Load " + _namesList.First()))
                {
                    Settings.LoadSettings(_namesList.First());
                }
            }
        }
    }
}
