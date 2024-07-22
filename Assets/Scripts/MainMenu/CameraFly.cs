using UnityEngine;
using DG.Tweening;

public class CameraFly : MonoBehaviour
{
    public MainMenuUI mainMenuUI;
    public Transform[] cameraPositions;
    public float transitionDuration;
    public float delayBetweenLoops;

    void Start()
    {
        // Start the camera fly-through sequence
        StartCameraFlySequence();
    }

    void StartCameraFlySequence()
    {
        MoveFirstSequence();
    }

    void MoveFirstSequence()
    {
        gameObject.transform.position = cameraPositions[0].position;
        mainMenuUI.FadeIn();
        transform.DOMove(cameraPositions[1].position, transitionDuration).SetEase(Ease.Linear).OnComplete(() => MoveSecondSequence());
        DOVirtual.DelayedCall(transitionDuration - 1, () => mainMenuUI.FadeOut());
    }

    void MoveSecondSequence()
    {
        gameObject.transform.position = cameraPositions[2].position;
        mainMenuUI.FadeIn();
        transform.DOMove(cameraPositions[3].position, transitionDuration).SetEase(Ease.Linear).OnComplete(() => MoveThirdSequence());
        DOVirtual.DelayedCall(transitionDuration - 1, () => mainMenuUI.FadeOut());
    }

    void MoveThirdSequence()
    {
        gameObject.transform.position = cameraPositions[4].position;
        mainMenuUI.FadeIn();
        transform.DOMove(cameraPositions[5].position, transitionDuration).SetEase(Ease.Linear).OnComplete(() => MoveFirstSequence());
        DOVirtual.DelayedCall(transitionDuration - 1, () => mainMenuUI.FadeOut());
    }
}
