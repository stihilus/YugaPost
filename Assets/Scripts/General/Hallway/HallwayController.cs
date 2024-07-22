using System;
using UnityEngine;

public class HallwayController : Singleton<HallwayController>
{
    public event Action<Transform, bool> OnCameraTriggered;

    public GameObject hallwayCamera;

    public GameObject shadow;

    public Transform firstTriggerPosition;

    public void Start()
    {
        HandleCameraTriggered(firstTriggerPosition, false);
        
        OnCameraTriggered += HandleCameraTriggered;
    }

    public void InvokeMoveCamera(Transform cameraTransform, bool shadowState)
    {
        OnCameraTriggered?.Invoke(cameraTransform, shadowState);
    }

    private void HandleCameraTriggered(Transform transform, bool shadowState)
    {
        ShowShadow(shadowState);
        
        hallwayCamera.transform.SetParent(transform);
        hallwayCamera.transform.localPosition = Vector3.zero;
        hallwayCamera.transform.localRotation = Quaternion.identity;
    }

    public void ShowShadow(bool state)
    {
        shadow.SetActive(state);
    }

    public void OnDestroy()
    {
        OnCameraTriggered -= HandleCameraTriggered;
    }
}
