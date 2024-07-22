using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class EndDay : MonoBehaviour, IWorldInteractable
{
    public TextMeshProUGUI text;

    public string textToDisplay;

    bool canEndDay = false;

    private void Start()
    {
        text.text = textToDisplay;

        GameEvents.OnPhotosBeingDeveloped += SetCanEndDay;
    }

    private void SetCanEndDay()
    {
        canEndDay = true;
    }
    
    public void FadeIn()
    {
        InputEventManager.Instance.OnInteractEvent.AddListener(DayEnded);
        text.DOFade(1, 0.2f);
    }

    public void FadeOut()
    {
        InputEventManager.Instance.OnInteractEvent.RemoveListener(DayEnded);
        text.DOFade(0, 0.2f);
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

    private void DayEnded()
    {
        StartCoroutine(DayEndedCoroutine());
    }

    IEnumerator DayEndedCoroutine()
    {
        if (canEndDay)
        {
            canEndDay = false;
            GameEvents.FadeOut();
            yield return new WaitForSeconds(1);
            GameEvents.FadeIn();
            GameEvents.DayEnded();
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnPhotosBeingDeveloped -= SetCanEndDay;
    }
}
