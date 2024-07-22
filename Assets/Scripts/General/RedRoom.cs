using TMPro;
using UnityEngine;
using DG.Tweening;

public class RedRoom : MonoBehaviour, IWorldInteractable
{
    public TextMeshProUGUI text;

    public string textToDisplay;

    private void Start()
    {
        text.text = textToDisplay;
    }
    
    public void FadeIn()
    {
        InputEventManager.Instance.OnInteractEvent.AddListener(DevelopPhotos);
        text.DOFade(1, 0.2f);
    }

    public void FadeOut()
    {
        InputEventManager.Instance.OnInteractEvent.RemoveListener(DevelopPhotos);
        text.DOFade(0, 0.2f);
    }

    private void DevelopPhotos()
    {
        text.text = "Photos developed!";
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeOut();
        }
    }
}
