using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

            GUI.Button(new Rect(0, 0, 200, 200), "test"); //test
        }
    }
}
