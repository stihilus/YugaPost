using UnityEngine;
using DG.Tweening;

public class Package : MonoBehaviour
{
    private Mail mail;
    [SerializeField] private Mesh[] possibleMeshes;
    [SerializeField] private MeshFilter mesh;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float speed;

    public void Init()
    {
        mesh.mesh = possibleMeshes[Random.Range(0, possibleMeshes.Length)];
        Rotate();
    }

    public void TakePackage()
    {
        FadeOut();
    }

    private void Rotate()
    {
        transform.DORotate(new Vector3(0, 360, 0), speed, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
    }

    private void FadeOut()
    {
        // fade material alpha
        meshRenderer.material.DOFade(0, .2f).onComplete += () => Destroy(gameObject);

        // Destroy(gameObject);
    }
}
