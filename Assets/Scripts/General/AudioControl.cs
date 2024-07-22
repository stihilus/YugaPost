using UnityEngine;

public static class AudioControl
{
    public static void Play(AudioSource audioSource, AudioClip audioClip)
    {
        if (audioSource.isPlaying) return;
        
        audioSource.PlayOneShot(audioClip);
    }

    public static void PlayRandom(AudioSource audioSource, AudioClip[] audioClips)
    {
        audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)]);
    }
}
