using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenManager : MonoBehaviour
{
    public GameObject titleScreen, pressToStartText;
    public Animator titleTextAnim, titleBombAnim, titlePirateAnim;

    public GameObject mainMenuScreen;
    public Animator mainMenuTitleAnimator;

    public GameObject pirate1, pirate2, pirate3;

    public GameObject optionsMenu;
    public GameObject optionsPanel;


    private int lastNumber;
    private void Update() //Logic update, wait for Space to start game
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GoToMainMenu();
        }
    }

    public void GoToMainMenu() //Handle the main menu screen presentation
    {
        SoundManager.Instance.PlayBombExplode();
        titleTextAnim.SetBool("MainMenu", true);
        titleBombAnim.SetBool("MainMenu", true);
        titlePirateAnim.SetBool("MainMenu", true);
        pressToStartText.SetActive(false);
        Invoke("DeactivateTitleScreen", 0.55f);
        Invoke("ShowMainMenu", 0.6f);
        Invoke("ActivateMenuTitle", 0.8f);
        Invoke("SnipRandomPirate", 0.9f);
    }

    private void DeactivateTitleScreen() //Deactive title screen
    {
        titleScreen.SetActive(false);
    }

    private void ShowMainMenu() //Shows the main menu
    {
        mainMenuScreen.SetActive(true);
    }
    private void ActivateMenuTitle() //Activates the menu title animation
    {
        mainMenuTitleAnimator.enabled = true;
    }

    public void GoToGameScene() //Start the game tutorial
    {
        SoundManager.Instance.PlayButtonClick();
        LevelLoader.Instance.LoadNextScene();
    }
    public void ExitGame() //Exit Game
    {
        SoundManager.Instance.PlayButtonClick();
        Application.Quit();
    }

    public void OpenOptionsMenu() //Open options menu in main menu
    {
        SoundManager.Instance.PlayButtonClick();
        optionsMenu.SetActive(true);
        optionsPanel.SetActive(true);
        CanvasRenderer canvas = optionsPanel.GetComponent<CanvasRenderer>();
        canvas.SetAlpha(0f);

        StartCoroutine(LerpPosition(optionsMenu, new Vector2(optionsMenu.transform.position.x, 525f), 0.35f));
        StartCoroutine(LerpFunction(canvas, 0.66f, 0.33f));
    }
    public void CloseOptionsMenu() //Close the options menu
    {
        StartCoroutine(DeactivateOptionsMenu());
        StartCoroutine(DeactivateOptionsBackground());
        StartCoroutine(LerpPosition(optionsMenu, new Vector2(optionsMenu.transform.position.x, -525f), 0.35f));
        StartCoroutine(LerpFunction(optionsPanel.GetComponent<CanvasRenderer>(), 0f, 0.33f));
    }
    IEnumerator LerpPosition(GameObject objectMoving, Vector2 targetPosition, float duration) //Lerp function of the options menu
    {
        float time = 0;
        Vector2 startPosition = objectMoving.transform.position;

        while (time < duration)
        {
            objectMoving.transform.position = Vector2.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        objectMoving.transform.position = targetPosition;
    }

    IEnumerator LerpFunction(CanvasRenderer elementToFade, float endValue, float duration) //Lerp function for the options background fade
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

    private IEnumerator DeactivateOptionsBackground() //Deactive the options backgroun
    {
        yield return new WaitForSeconds(0.4f);
        optionsPanel.SetActive(false);
    }


    private IEnumerator DeactivateOptionsMenu() //Deactive the options menu
    {
        yield return new WaitForSeconds(0.4f);
        optionsMenu.SetActive(false);
    }

    private void SnipRandomPirate() //Handle the pirates looking in the main menu
    {
        int randomNumber = UnityEngine.Random.Range(1, 4);

        if (lastNumber.Equals(randomNumber))
        {
            while(lastNumber.Equals(randomNumber))
                randomNumber = UnityEngine.Random.Range(1, 4);
        }
            lastNumber = randomNumber;

        switch (randomNumber)
        {
            case 1:
                pirate1.SetActive(true);
                StartCoroutine(DisablePirate(randomNumber));
                break;
            case 2:
                pirate2.SetActive(true);
                StartCoroutine(DisablePirate(randomNumber));
                break;
            case 3:
                pirate3.SetActive(true);
                StartCoroutine(DisablePirate(randomNumber));
                break;
        }
    }
    private IEnumerator DisablePirate(int number) //Disable the pirate that peeked and choose another
    {
        yield return new WaitForSeconds(2f);

        switch (number)
        {
            case 1:
                pirate1.SetActive(false);
                break;
            case 2:
                pirate2.SetActive(false);
                break;
            case 3:
                pirate3.SetActive(false);
                break;
        }

        SnipRandomPirate();
    }
}
