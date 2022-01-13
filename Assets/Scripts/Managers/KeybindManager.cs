using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    private string bindName;
    public Dictionary<string, KeyCode> Keybinds { get; private set; }
    private static KeybindManager instance;
    private static bool initialized = false;

    public static KeybindManager Instance //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<KeybindManager>();
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (!initialized) //Check if ONE was initialized
        {
            initialized = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject); //Destroy replicas of the object that are created
                                          //every time the player goes to the main menu
        }
    }
    private void Start()
    {
        Keybinds = new Dictionary<string, KeyCode>();

        if (PlayerPrefs.HasKey("Left")) { //If there's already one keybind saved, then recover all
            BindKey("Left", (KeyCode)PlayerPrefs.GetInt("Left"));
            BindKey("Right", (KeyCode)PlayerPrefs.GetInt("Right"));
            BindKey("Jump", (KeyCode)PlayerPrefs.GetInt("Jump"));
            BindKey("Bomb", (KeyCode)PlayerPrefs.GetInt("Bomb"));
            BindKey("Pause", (KeyCode)PlayerPrefs.GetInt("Pause"));
            return;
        }
        //Else default all keybinds

        BindKey("Left", KeyCode.A);
        BindKey("Right", KeyCode.D);
        BindKey("Jump", KeyCode.Space);
        BindKey("Bomb", KeyCode.E);
        BindKey("Pause", KeyCode.Escape);
        PlayerPrefs.SetInt("SavedKeybinds", 1);

    }

    public void BindKey(string key, KeyCode keyBind) //Bind keybind to key
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            PlayerPrefs.SetInt(key, (int) keyBind);
            OptionsManager.Instance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind)) //If keyCode is already in use, if so remove the other usage
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key; 

            currentDictionary[myKey] = KeyCode.None;
            PlayerPrefs.SetInt(myKey, (int) KeyCode.None);
            OptionsManager.Instance.UpdateKeyText(key, KeyCode.None);
        }

        currentDictionary[key] = keyBind;
        PlayerPrefs.SetInt(key, (int)keyBind);
        OptionsManager.Instance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    public void KeyBindOnClick(string bindName) //Check for user mouse click
    {
        SoundManager.Instance.PlayButtonClick();
        this.bindName = bindName;
        OptionsManager.Instance.KeyBindOnClick(bindName);
    }

    private void OnGUI() //Handle keybind insertion after starting the bind
    {
        if(bindName != string.Empty)
        {
            Event e = Event.current;
            if (e.isKey)
            {
                BindKey(bindName, e.keyCode);
            }
        }
    }
}
