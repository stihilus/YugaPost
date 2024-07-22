using UnityEngine;

public class NPCFootsteps : MonoBehaviour
{
    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private float footstepVolume = .05f;

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footsteps.Length > 0)
            {
                var index = Random.Range(0, footsteps.Length);
                AudioSource.PlayClipAtPoint(footsteps[index], transform.position, footstepVolume);
            }
        }
    }
}
