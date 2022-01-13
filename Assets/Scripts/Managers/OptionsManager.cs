using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    private static OptionsManager instance;
    private GameObject[] keybindButtons;
    public static OptionsManager Instance //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<OptionsManager>();
            }
            return instance;
        }
    }

    private void Awake() //Get keybinds buttons
    {
        keybindButtons = GameObject.FindGameObjectsWithTag("Keybind");
    }

    public void UpdateKeyText(string key, KeyCode keyCode) //Update keybind text
    {
        TextMeshProUGUI tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = keyCode.ToString();
    }
    public void KeyBindOnClick(string key) //Update keybind text while binding new key
    {
        TextMeshProUGUI tmp = Array.Find(keybindButtons, x => x.name == key).GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = "----";
    }
}
