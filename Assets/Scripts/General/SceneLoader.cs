using DG.Tweening;

public static class SceneLoader
{
    public static void LoadScene(string sceneName, float delay = 1)
    {
        // Fade out the UI
        GameEvents.FadeOut();

        // Load the game scene
        DOVirtual.DelayedCall(delay, () => UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName));
    }
}
