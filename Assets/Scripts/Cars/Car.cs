using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Car : MonoBehaviour
{
    private DayNightCycle dayNightCycle;
    
    private bool isMoving;
    private int nextLocationIndex = 0;

    [Header("References")]
    [SerializeField] private GameObject lights;

    [Header("Car Settings")]
    [SerializeField] private float constantMoveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private NPCPath path;

    private void Start()
    {
        dayNightCycle = DayNightCycle.Instance;

        dayNightCycle.onNightStart.AddListener(EnableLights);
        dayNightCycle.onDayStart.AddListener(DisableLights);
        
        transform.position = path.locations[0].position;
        MoveToLocation(path.locations[nextLocationIndex]);
    }

    private void EnableLights()
    {
        lights.SetActive(true);
    }

    private void DisableLights()
    {
        lights.SetActive(false);
    }

    private void MoveToLocation(Transform location)
    {
        // Calculate the direction to the next location
        Vector3 direction = (location.position - transform.position).normalized;

        // Rotate towards the next location
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.DORotateQuaternion(targetRotation, rotationSpeed);
        }

        // Calculate the distance between the current and next location
        float distance = Vector3.Distance(transform.position, location.position);

        // Calculate the duration based on the constant move speed
        float duration = distance / constantMoveSpeed;

        transform.DOMove(location.position, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                FindNextLocation();
            });
    }

    private void FindNextLocation()
    {
        nextLocationIndex++;
        if (nextLocationIndex >= path.locations.Length)
        {
            nextLocationIndex = 0; // Loop back to the first transform
        }
        MoveToLocation(path.locations[nextLocationIndex]);
    }

    private void OnDestroy()
    {
        dayNightCycle.onNightStart.RemoveListener(EnableLights);
        dayNightCycle.onDayStart.RemoveListener(DisableLights);
    }
}
