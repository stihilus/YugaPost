using TMPro;
using UnityEngine;
using DG.Tweening;

public class EnterLevel : MonoBehaviour, IWorldInteractable
{
    public string levelName;
    public TextMeshProUGUI text;

    public string textToDisplay;

    private void Start()
    {
        text.text = textToDisplay;
    }
    
    public void FadeIn()
    {
        InputEventManager.Instance.OnInteractEvent.AddListener(LoadLevel);
        text.DOFade(1, 0.2f);
    }

    public void FadeOut()
    {
        InputEventManager.Instance.OnInteractEvent.RemoveListener(LoadLevel);
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

    private void LoadLevel()
    {
        SceneLoader.LoadScene(levelName);
    }
}
