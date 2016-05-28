using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityInterface.SettingTemplates;
using ImportantClasses;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityInterface
{
    internal class Menu : MonoBehaviour
    {
        private List<string> _namesList; //saves place in menu which is actually shown
        private Vector2 _itemScrollPosition; //the scroll Position for the Menuitems
        private Vector2 _settingScrollPosition; //the scroll position for the settings
        private Vector2 _topBarScrollPosition; //the scroll position of the bar on the top
        private bool _showSettings = false; //false = Main Menu; true = settings;

        //setting lists
        private List<string> _menuItems;
        private Dictionary<string, object> _namesDictionary;

        //Define Size
        private float _padding;
        private float _contentWidth;
        private float _topBarHeight;
        private float _topBarButtonWidth;
        private float _buttonHeight;
        private float _contentHeight;
        private float _menuItemWidth;
        private float _menuSettingWidth;
        
        /// <summary>
        /// called by unity before rendering the first frame
        /// </summary>
        private void Start()
        {
            _namesList = new List<string>();
            Settings.Initialize();
            CalculateSize();
            _itemScrollPosition = new Vector2(0, 0);
            _settingScrollPosition = new Vector2(0, 0);
            _topBarScrollPosition = new Vector2(0, 0);
        }
        
        /// <summary>
        /// called by unity when an action to the userinterface happend
        /// </summary>
        private void OnGUI()
        {
            if (!_showSettings)
                DrawMainMenu();
            else //Draw Settings
            {
                //get the items and settings that are actually shown
                _menuItems = Settings.GetMenuItems(_namesList.ToArray());
                _namesDictionary = Settings.GetMenuSettings(_namesList.ToArray());

                //calculate the height of the settings for the scrollView
                float settingContentHeight = 0;
                foreach (KeyValuePair<string, object> keyValuePair in _namesDictionary)
                {
                    if (keyValuePair.Value.GetType().IsArray)
                    {
                        //the direct conversion (values = (object[]) oldValue) is not possible
                        object[] values = ((IEnumerable)keyValuePair.Value).Cast<object>().ToArray();
                        settingContentHeight += (SettingTemplate.GetRelativeHeight(typeof(string)) +
                                  values.Length * SettingTemplate.GetRelativeHeight(values[0].GetType())) * _contentHeight;
                    }
                    else
                        settingContentHeight += SettingTemplate.GetRelativeHeight(keyValuePair.Value.GetType()) * _contentHeight;
                }
                settingContentHeight += _buttonHeight + _padding + 20; //height of the buttons on the bottom + the _padding between button and settings + height of the scrollBar

                //Begin drawing of the menu
                GUI.BeginGroup(new Rect(_padding, _padding, _contentWidth, _contentHeight)); //content; _padding to the edge

                //Bar on the top
                _topBarScrollPosition = GUI.BeginScrollView(new Rect(0, 0, _contentWidth, _topBarHeight),
                    _topBarScrollPosition, new Rect(0, 0, _contentWidth, _topBarHeight)); 
                DrawTopBar();
                GUI.EndScrollView();

                //MenuItems
                _itemScrollPosition =
                    GUI.BeginScrollView(new Rect(0, _topBarHeight, _menuItemWidth + 20, _contentHeight - _topBarHeight),
                        _itemScrollPosition, new Rect(0, 0, _menuItemWidth, _menuItems.Count*(_buttonHeight + _padding))); 
                DrawMenuItems();
                GUI.EndScrollView();//End MenuItems
                
                //Settings
                _settingScrollPosition =
                    GUI.BeginScrollView(new Rect(_menuItemWidth + _padding, _topBarHeight, _menuSettingWidth+20,
                        _contentHeight - _topBarHeight), _settingScrollPosition, new Rect(0, 0, _menuSettingWidth, settingContentHeight));
                DrawSettings();
                GUI.EndScrollView();//End Settings

                GUI.EndGroup(); //End Content
            }
        }

        /// <summary>
        /// calcutates the size of the controls according to the screen size
        /// </summary>
        private void CalculateSize()
        {
            _padding = (Screen.width + Screen.height) / 200f; //1% _padding to the edge of the screen
            _contentWidth = Screen.width - (2 * _padding); //width of the Screeen minus the _padding
            _contentHeight = Screen.height - (2 * _padding); //height of the Screen minus the _padding
            _topBarHeight = Screen.height / 100 * 12; //12% Height for the Bar on the Top
            _buttonHeight = _topBarHeight - _padding;
            _topBarButtonWidth = _contentWidth / 8;
            _menuItemWidth = _contentWidth / 5 - 20; //20% of the width for the MenuItems; 20 is subtracted for the scrollBar
            _menuSettingWidth = _contentWidth - _menuItemWidth - _padding - 20; //80% minus the _padding for the Settings; 20 is subtracted for the scrollBar
        }

        /// <summary>
        /// Draws the Main Menu
        /// </summary>
        private void DrawMainMenu()
        {
            //Start Button
            if (GUI.Button(new Rect(_menuItemWidth, _topBarHeight, _contentWidth - 2*_menuItemWidth, _buttonHeight),
                "Start Race"))
            {
                SceneManager.LoadScene("Simulator");
            }

            //Settings Button
            if ( GUI.Button(
                    new Rect(_menuItemWidth, _topBarHeight + _buttonHeight + _padding, _contentWidth - 2*_menuItemWidth,
                        _buttonHeight), "Settings"))
            {
                _showSettings = true;
            }

            //Quit Application
            if (
                GUI.Button(
                    new Rect(_menuItemWidth, _topBarHeight + (_buttonHeight + _padding)*2.5f,
                        _contentWidth - 2*_menuItemWidth,
                        _buttonHeight), "Quit"))
            {
                Application.Quit();
            }
        }

        /// <summary>
        /// Draws the bar on the top of the screen to go back
        /// </summary>
        private void DrawTopBar()
        {
            //back to the main menu
            if (GUI.Button(new Rect(0, 0, _topBarButtonWidth, _buttonHeight), "main"))
            {
                _namesList.Clear();
                _showSettings = false;
            }

            //back to the settings start page
            //show just if the page is actually not showed
            if(_namesList.Count > 0 && GUI.Button(new Rect(_topBarButtonWidth + _padding, 0,_topBarButtonWidth, _buttonHeight),"settings"))
                _namesList.Clear();

            //back to a menuitem in the hierachy
            for (int i = 0; i < _namesList.Count - 1; i++)
            {
                if(GUI.Button(new Rect((i+2)*(_topBarButtonWidth+_padding), 0, _topBarButtonWidth, _buttonHeight),_namesList.ElementAt(i)))
                {
                    _namesList = _namesList.GetRange(0, i + 1);
                    break;
                }
            }
        }

        /// <summary>
        /// Draws the Buttons on the side of the Menu
        /// </summary>
        private void DrawMenuItems()
        {
            int i = 0;
            foreach (string menuItem in _menuItems)
            {
                if (GUI.Button(new Rect(0, i++ * (_topBarHeight + _padding), _menuItemWidth, _buttonHeight), menuItem))
                {
                    _namesList.Add(menuItem);
                    break;
                }
            }
        }

        /// <summary>
        /// Draws the Settings
        /// </summary>
        private void DrawSettings()
        {
            float posY = 0; //the position in Y-direction for the next control
            foreach (KeyValuePair<string, object> keyValuePair in _namesDictionary)
            {
                float height; //the height of the control
                bool valueChanged = false;

                object newValue = null;
                if (keyValuePair.Value.GetType().IsArray) //Draw an array
                {
                    //the direct conversion (values = (object[]) oldValue) is not possible
                    object[] values = ((IEnumerable) keyValuePair.Value).Cast<object>().ToArray();
                    height = (SettingTemplate.GetRelativeHeight(typeof(string)) +
                              values.Length*SettingTemplate.GetRelativeHeight(values[0].GetType()))*_contentHeight;
                    if (values.Length > 0)
                        newValue = SettingTemplate.DrawArray(values, keyValuePair.Key,
                            new Rect(0, posY, _menuSettingWidth, height));
                    if (newValue != null && !newValue.Equals(values))
                        valueChanged = true;
                }
                else //Draw a value other than arrays
                {
                    height = SettingTemplate.GetRelativeHeight(keyValuePair.Value.GetType())*_contentHeight;
                    newValue = SettingTemplate.Draw(keyValuePair.Value, keyValuePair.Key,
                        new Rect(0, posY, _menuSettingWidth, height));

                    if (newValue != null && !newValue.Equals(keyValuePair.Value))
                        valueChanged = true;
                }

                if(valueChanged)//if the value was changed: store it temporary
                {
                    List<string> tempList = new List<string>(_namesList); //this list is to add a Value temporary to _namesList
                    tempList.Add(keyValuePair.Key);
                    Settings.ChangeSettingTomporary(tempList.ToArray(), newValue); //add the changed value to the buffer of temporary changes
                }

                posY += height + _padding/2; //calculate the position in Y-direction
            }

            DrawSettingButtons(posY);
        }

        /// <summary>
        /// Draws the buttons under the settings for loading and saving
        /// </summary>
        /// <param name="posY">the position of the buttons in Y-Direction so that they are not collide with other controls</param>
        private void DrawSettingButtons(float posY)
        {
            if (Settings.HasTemporaryChanges()) //show the buttons just if there are changes
            {
                if (GUI.Button(new Rect(0, posY, _menuSettingWidth*0.24f, _buttonHeight),
                    "save all Settings"))
                {
                    Settings.SaveTemporaryChanges();
                    Settings.SaveAllSettings(
                        EditorUtility.DisplayDialog("create new Files?",
                        "Do you want to crate new Files or do you want to overwrite the last-used files", "new Files", "overwrite"));
                }

                //Draw the Discard changes Button just if there are changes
                if ( GUI.Button(new Rect(_menuSettingWidth*0.25f, posY, _menuSettingWidth*0.25f, _buttonHeight),
                        "Discard changes"))
                    Settings.DiscardTemporaryChanges();
            }
            if (_namesList.Count != 0) //show just if a Menuitem is selected
            {
                //save button
                if (GUI.Button(new Rect(_menuSettingWidth * 0.51f, posY, _menuSettingWidth * 0.24f, _buttonHeight),
                    "Save " + _namesList.First() + " under"))
                {
                    Settings.SaveTemporaryChanges(_namesList.First());
                    Settings.SaveSetting(_namesList.First());
                }

                //load button
                if (GUI.Button(new Rect(_menuSettingWidth * 0.76f, posY, _menuSettingWidth * 0.24f, _buttonHeight),
                    "Load " + _namesList.First()))
                {
                    Settings.LoadSettings(_namesList.First());
                }
            }

        }
    }
}
