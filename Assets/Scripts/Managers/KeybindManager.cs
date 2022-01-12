using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeybindManager : MonoBehaviour
{
    private static KeybindManager instance;
    private string bindName;
    public Dictionary<string, KeyCode> Keybinds { get; private set; }
    public static KeybindManager Instance
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

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        Keybinds = new Dictionary<string, KeyCode>();

        BindKey("Left", KeyCode.A);
        BindKey("Right", KeyCode.D);
        BindKey("Jump", KeyCode.Space);
        BindKey("Bomb", KeyCode.E);
        BindKey("Pause", KeyCode.Escape);
    }

    public void BindKey(string key, KeyCode keyBind)
    {
        Dictionary<string, KeyCode> currentDictionary = Keybinds;

        if (!currentDictionary.ContainsKey(key))
        {
            currentDictionary.Add(key, keyBind);
            OptionsManager.Instance.UpdateKeyText(key, keyBind);
        }
        else if (currentDictionary.ContainsValue(keyBind)) //If keyCode is already in use, if so remove the other usage
        {
            string myKey = currentDictionary.FirstOrDefault(x => x.Value == keyBind).Key; 

            currentDictionary[myKey] = KeyCode.None;
            OptionsManager.Instance.UpdateKeyText(key, KeyCode.None);
        }

        currentDictionary[key] = keyBind;
        OptionsManager.Instance.UpdateKeyText(key, keyBind);
        bindName = string.Empty;
    }

    public void KeyBindOnClick(string bindName)
    {
        this.bindName = bindName;
        OptionsManager.Instance.KeyBindOnClick(bindName);
    }

    private void OnGUI()
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
