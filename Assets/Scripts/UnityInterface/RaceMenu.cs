using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityInterface
{
    public class RaceMenu : MonoBehaviour
    {
        public static bool ShowMenu { get; set; }

        private void Start()
        {
            ShowMenu = false;
        }

        private void OnGUI()
        {
            if (ShowMenu)
            {
                if (GUI.Button(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/4),
                    "Continue Race"))
                {
                    ShowMenu = false;
                }
                if (GUI.Button(new Rect(Screen.width/4, Screen.height/2, Screen.width/2, Screen.height/4),
                    "To Main Menu"))
                {
                    SceneManager.LoadScene("MainMenu");
                    //SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainMenu"));
                    //SceneManager.UnloadScene("Simulator");
                }
            }
            else
            {
                if (GUI.Button(new Rect(10, 10, Screen.width/5, Screen.height/15), "Menu"))
                    ShowMenu = true;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                ShowMenu = !ShowMenu;
        }
    }
}
