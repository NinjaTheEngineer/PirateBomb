using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private GameObject volumeSettings;
    [SerializeField] private GameObject gameOverText;

    [SerializeField] private Animator heart_1, heart_2, heart_3;
    [SerializeField] private Image bombIcon;
    [SerializeField] private TextMeshProUGUI bombAmount;


    private static UIManager instance;

    public static UIManager Instance //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
            }
            return instance;
        }
    }
    private void Awake() //Initialize singleton
    {
        instance = this;
    }

    public void UpdatePlayerHealth(int healthAmount) //Update players health UI
    {
        switch (healthAmount)
        {
            case 2:
                heart_3.SetTrigger("loseHeart");
                break;
            case 1:
                heart_2.SetTrigger("loseHeart");
                break;
            case 0:
                heart_1.SetTrigger("loseHeart");
                break;
        }
    }

    public void UpdateBombAmount(int bombsAmount) //Update bomb amount UI 
    {
        bombAmount.SetText("x" + bombsAmount.ToString());
    }

    public void UpdateBombReloadTime(float time) //Update bomb reload UI
    {
        bombIcon.fillAmount = time;
    }

    public void UpdateGameOver() //Handle game over dialog
    {
        titleText.text = "Defeated";
        volumeSettings.SetActive(false);
        gameOverText.SetActive(true);
    }

}
