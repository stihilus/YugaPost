using UnityEngine;
using DG.Tweening;
using TMPro;

public class Mailbox : MonoBehaviour, IWorldInteractable
{
    public TextMeshProUGUI addressText;
    public string address;
    public int number;

    private void Awake()
    {
        addressText.text = address + " " + number.ToString();
    }

    public void FadeIn()
    {
        addressText.DOFade(1, 0.2f);
    }

    public void FadeOut()
    {
        addressText.DOFade(0, 0.2f);
    }
}
