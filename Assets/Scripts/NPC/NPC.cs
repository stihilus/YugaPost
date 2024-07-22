using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class NPC : MonoBehaviour
{
    private bool isMoving;
    private int nextLocationIndex = 0;

    [Header("NPC Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float constantMoveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private NPCPath path;

    private void Awake()
    {
        transform.position = path.locations[0].position;
    }

    private void Start()
    {
        MoveToLocation(path.locations[nextLocationIndex]);
    }

    private void MoveToLocation(Transform location)
    {
        // Calculate the direction to the next location
        Vector3 direction = (location.position - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            // Rotate towards the next location
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(targetRotation, rotationSpeed);
        }
        
        // Calculate the distance between the current and next location
        float distance = Vector3.Distance(transform.position, location.position);

        // Calculate the duration based on the constant move speed
        float duration = distance / constantMoveSpeed;
        
        Walk();
        
        transform.DOMove(location.position, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => {
                RandomWaitUntilNextLocation();
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

    private void RandomWaitUntilNextLocation()
    {
        Idle();
        float randomWaitTime = Random.Range(1f, 5f);
        Invoke("FindNextLocation", randomWaitTime);
    }

    private void Walk()
    {
        animator.SetFloat("MotionSpeed", 1f);
        DOTween.To(() => animator.GetFloat("Speed"), x => animator.SetFloat("Speed", x), 2f, 0.5f);
    }

    private void Idle()
    {
        animator.SetFloat("MotionSpeed", 1f);
        DOTween.To(() => animator.GetFloat("Speed"), x => animator.SetFloat("Speed", x), 0f, 0.5f);
    }
}
