using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class PlayerUI : MonoBehaviour
{
    public float fadeDuration;
    
    public TextMeshProUGUI generalText;

    public void SetGeneralText(string text)
    {
        generalText.text = text;
    }

    public void FadeIn()
    {
        generalText.DOFade(1, fadeDuration).OnComplete(() =>
        {
            DOVirtual.DelayedCall(4f, FadeOut);
        });
    }

    public void FadeOut()
    {
        generalText.DOFade(0, fadeDuration);
    }
}
