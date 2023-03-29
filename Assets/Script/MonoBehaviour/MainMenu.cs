using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool quality = false;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 200, 50), "Start the game"))
        {
            Application.LoadLevel("SampleScene");
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 50), "Change Graphics Quality"))
        {
            if (!quality)
            {
                quality = true;
            }
            else
            {
                quality = false;
            }
        }

        if (quality)
        {
            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 -150, 200, 50), "Fastest"))
            {
                QualitySettings.currentLevel = QualityLevel.Fastest;
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 - 100, 200, 50), "Fast"))
            {
                QualitySettings.currentLevel = QualityLevel.Fast;
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 - 50, 200, 50), "Simple"))
            {
                QualitySettings.currentLevel = QualityLevel.Simple;
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2, 200, 50), "Good"))
            {
                QualitySettings.currentLevel = QualityLevel.Good;
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 + 50, 200, 50), "Beautiful"))
            {
                QualitySettings.currentLevel = QualityLevel.Beautiful;
            }

            if (GUI.Button(new Rect(Screen.width / 2 + 150, Screen.height / 2 + 100, 200, 50), "Fantastic"))
            {
                QualitySettings.currentLevel = QualityLevel.Fantastic;
            }

            if (Input.GetKeyDown("escape"))
            {
                quality = false;
            }
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 200, 50), "Quit Game"))
        {
            Application.Quit();
        }
    }
}
