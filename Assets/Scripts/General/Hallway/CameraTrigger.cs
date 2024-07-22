using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public GameObject hallwayCamera;
    public Transform cameraTransform;
    public bool shadowState;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HallwayController.Instance.InvokeMoveCamera(cameraTransform, shadowState);
        }
    }
}
