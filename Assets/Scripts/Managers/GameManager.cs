using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool GameStarted { get; private set; }
    public GameObject player;

    [SerializeField]
    private bool isTutorial;

    public bool IsTutorial() { return isTutorial; }

    private bool openingMenu;
    public GameObject optionsMenu;
    public GameObject optionsPanel;
    public GameObject door;
    private static GameManager instance;
    public static GameManager Instance //Singleton
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private void Awake() 
    {
        GameStarted = false;
    }

    private void Start() //Initialize entering level
    {
        player.SetActive(true);
        StartCoroutine(EnterLevel());
    }

    private IEnumerator EnterLevel() //Handle enter level animations
    {
        yield return new WaitForSeconds(1.5f);
        SoundManager.Instance.PlayDoorOpen();
        door.GetComponent<Animator>().SetBool("open", true);
        player.GetComponent<Animator>().SetTrigger("doorOut");
        yield return new WaitForSeconds(0.5f);
        GameStarted = true;
    }

    public void OpenMainMenu() //Go to main menu
    {
        SoundManager.Instance.PlayButtonClick();
        CloseOptionsMenu();
        LevelLoader.Instance.LoadMainMenu();
    }

    public void RestartLevel() //Restart level
    {
        SoundManager.Instance.PlayButtonClick();
        CloseOptionsMenu();
        LevelLoader.Instance.RestartLevel();
    }

    public void GameOver() //Set game over
    {
        GameStarted = false;
        OpenOptionsMenu();
        UIManager.Instance.UpdateGameOver();
    }

    public void OpenOptionsMenu() //Open options menu in game
    {
        openingMenu = true;
        optionsMenu.SetActive(true);
        optionsPanel.SetActive(true);
        CanvasRenderer canvas = optionsPanel.GetComponent<CanvasRenderer>();
        canvas.SetAlpha(0f);

        StartCoroutine(LerpPosition(optionsMenu, new Vector2(optionsMenu.transform.position.x, 525f), 0.35f));
        StartCoroutine(LerpFunction(canvas, 0.66f, 0.33f));
    }
    public void CloseOptionsMenu() //Close options menu
    {
        SoundManager.Instance.PlayButtonClick();
        openingMenu = false;
        StartCoroutine(DeactivateOptionsMenu());
        StartCoroutine(DeactivateOptionsBackground());
        StartCoroutine(LerpPosition(optionsMenu, new Vector2(optionsMenu.transform.position.x, -525f), 0.35f));
        StartCoroutine(LerpFunction(optionsPanel.GetComponent<CanvasRenderer>(), 0f, 0.33f));
    }
    IEnumerator LerpPosition(GameObject objectMoving, Vector2 targetPosition, float duration) //Lerp function for the options menu transition
    {
        Time.timeScale = 1f;
        float time = 0;
        Vector2 startPosition = objectMoving.transform.position;

        while (time < duration)
        {
            objectMoving.transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        objectMoving.transform.position = targetPosition;
        if (openingMenu)
        {
            Time.timeScale = 0f;
        }
    }

    IEnumerator LerpFunction(CanvasRenderer elementToFade, float endValue, float duration) //Lerp for the fade of the options background
    {
        float time = 0;
        float startValue = elementToFade.GetAlpha();

        while (time < duration)
        {
            elementToFade.SetAlpha(Mathf.Lerp(startValue, endValue, time / duration));

            time += Time.deltaTime;
            yield return null;
        }
        elementToFade.SetAlpha(endValue);
    }

    private IEnumerator DeactivateOptionsBackground() //Deactivates background
    {
        yield return new WaitForSeconds(0.4f);
        optionsPanel.SetActive(false);
    }


    private IEnumerator DeactivateOptionsMenu() //Deactivates options menu
    {
        yield return new WaitForSeconds(0.4f);
        optionsMenu.SetActive(false);
    }

}
