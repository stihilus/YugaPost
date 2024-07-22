using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    private void Update()
    {
        // Calculate the rotation to face the camera
        Quaternion targetRotation = Quaternion.LookRotation(mainCamera.transform.forward, mainCamera.transform.up);

        // Apply the rotation to the UI element
        transform.rotation = targetRotation;
    }
}