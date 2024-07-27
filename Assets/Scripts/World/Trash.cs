using DG.Tweening;
using TMPro;
using UnityEngine;

public class Trash : MonoBehaviour, IWorldInteractable
{
    public TextMeshProUGUI info;

    public string displayOnEnter;
    public string displayOnDrop;
    
    public void FadeIn()
    {
        info.text = displayOnEnter;
        info.DOFade(1, 0.2f);
    }

    public void FadeOut()
    {
        info.DOFade(0, 0.2f);
    }

    public void DropTrash()
    {
        info.text = displayOnDrop;

        SaveLoad.DeleteAllData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeIn();
            InputEventManager.Instance.OnInteractEvent.AddListener(DropTrash);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeOut();
            InputEventManager.Instance.OnInteractEvent.RemoveListener(DropTrash);
        }
    }
}
