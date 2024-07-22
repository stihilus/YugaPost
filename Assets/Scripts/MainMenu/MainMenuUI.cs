using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuUI : MonoBehaviour
{
    public Image fader;

    public void StartGame()
    {
        // Fade out the UI
        FadeOut();

        // Load the game scene
        DOVirtual.DelayedCall(1, () => UnityEngine.SceneManagement.SceneManager.LoadScene("Playground"));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void FadeIn()
    {
        fader.DOFade(0, 1).From(1).SetEase(Ease.Linear);
    }

    public void FadeOut()
    {
        fader.DOFade(1, 1).SetEase(Ease.Linear);
    }
}
