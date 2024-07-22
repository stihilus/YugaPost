using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FadeController : MonoBehaviour
{
    public Image fader;

    private void Awake()
    {
        GameEvents.OnFadeIn += FadeIn;
        GameEvents.OnFadeOut += FadeOut;
    }

    private void Start()
    {
        FadeIn();
    }

    /// <summary>
    /// Fade in the screen from black to white
    /// </summary>
    private void FadeIn()
    {
        fader.gameObject.SetActive(true);
        fader.DOFade(0, 1f)
            .SetEase(Ease.Linear)
            .From(1f)
            .OnComplete(() => fader.gameObject.SetActive(false));
    }

    /// <summary>
    /// Fade out the screen from white to black
    /// </summary>
    private void FadeOut()
    {
        fader.gameObject.SetActive(true);
        fader.DOFade(1, 1f)
            .SetEase(Ease.Linear).From(0f);
    }

    private void OnDestroy()
    {
        GameEvents.OnFadeIn -= FadeIn;
        GameEvents.OnFadeOut -= FadeOut;
    }
}
