using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro tutorialText_1;
    [SerializeField] private TextMeshPro tutorialText_2;
    [SerializeField] private TextMeshPro tutorialText_3;
    [SerializeField] private TextMeshPro exitText;

    bool startEnemy = false;

    public Dictionary<string, KeyCode> Keybinds { get; private set; }

    private void Start() //Start the tutorial texts
    {
        Keybinds = KeybindManager.Instance.Keybinds;

        tutorialText_1.SetText($"" +
            $"{"Press\n".AddColor(Color.white)}" +
            $"{(Keybinds["Left"].ToString()).AddColor(Color.green)}" +
            $"{" and ".AddColor(Color.white)}" +
            $"{(Keybinds["Right"].ToString()).AddColor(Color.green)}" +
            $"{"\nto Move".AddColor(Color.white)}");

        tutorialText_2.SetText($"" +
            $"{"Press\n".AddColor(Color.white)}" +
            $"{(Keybinds["Jump"].ToString()).AddColor(Color.green)}" +
            $"{"\nto Jump".AddColor(Color.white)}");

        tutorialText_3.SetText($"" +
            $"{"To place a Bomb\nPress ".AddColor(Color.white)}" +
            $"{(Keybinds["Bomb"].ToString()).AddColor(Color.green)}" +
            $"{"\nTo Throw a Bomb\nHold ".AddColor(Color.white)}" +
            $"{(Keybinds["Bomb"].ToString()).AddColor(Color.green)}");

        exitText.SetText($"" +
            $"{"Press\n".AddColor(Color.white)}" +
            $"{(Keybinds["Bomb"].ToString()).AddColor(Color.green)}" +
            $"{"\nTo Exit".AddColor(Color.white)}");
    }

    private void OnTriggerEnter2D(Collider2D collision) //Start the enemy for the tutorial
    {
        if (GameManager.Instance.IsTutorial() && !startEnemy)
        {
            if (collision.tag.Equals("Player"))
            {
                GameObject.FindGameObjectWithTag("Enemy").GetComponent<Enemy_Bald_Pirate>().ChangeStateToIdleState();
                startEnemy = true;
            }
        }
    }
}
public static class StringExtensions //Extensions for textmeshpro text colors
{
    public static string AddColor(this string text, Color col) => $"<color={ColorHexFromUnityColor(col)}>{text}</color>";
    public static string ColorHexFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
}

