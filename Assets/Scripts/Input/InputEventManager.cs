using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem.Utilities;

public class InputEventManager : Singleton<InputEventManager>
{
    public BaseInputs inputActions;

    public UnityEvent<Vector2> OnMoveEvent;
    public UnityEvent<Vector2> OnLookEvent;
    public UnityEvent OnJumpEvent;
    public UnityEvent OnInteractEvent;
    public UnityEvent OnOpenUIEvent;
    public UnityEvent OnCloseUIEvent;
    public UnityEvent OnNavBottomLeftEvent;
    public UnityEvent OnNavBottomRightEvent;
    public UnityEvent OnNavMiddleLeftEvent;
    public UnityEvent OnNavMiddleRightEvent;
    public UnityEvent OnMapOpen;
    public UnityEvent OnInventoryOpen;
    public UnityEvent OnMapEvent;
    public UnityEvent OnPhotoModeEvent;
    public UnityEvent OnTakePhotoEvent;

    protected override void Awake()
    {
        base.Awake();
        
        inputActions = new BaseInputs();
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Interact.performed += OnInteract;

        inputActions.Player.PhotoMode.performed += OnPhotoMode;
        inputActions.Player.TakePhoto.performed += OnTakePhoto;
        
        inputActions.Player.OpenUI.performed += OnOpenUI;
        inputActions.UI.CloseUI.performed += OnCloseUI;
        inputActions.UI.NavBottomLeft.performed += OnNavBottomLeft;
        inputActions.UI.NavBottomRight.performed += OnNavBottomRight;
        inputActions.UI.NavMiddleLeft.performed += OnNavMiddleLeft;
        inputActions.UI.NavMiddleRight.performed += OnNavMiddleRight;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        OnMoveEvent.Invoke(moveInput);
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        OnLookEvent.Invoke(lookInput);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        OnJumpEvent.Invoke();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        OnInteractEvent.Invoke();
    }

    private void OnPhotoMode(InputAction.CallbackContext context)
    {
        OnPhotoModeEvent.Invoke();
    }

    private void OnTakePhoto(InputAction.CallbackContext context)
    {
        OnTakePhotoEvent.Invoke();
    }

    private void OnOpenUI(InputAction.CallbackContext context)
    {
        OnOpenUIEvent.Invoke();
    }

    private void OnCloseUI(InputAction.CallbackContext context)
    {
        OnCloseUIEvent.Invoke();
    }

    private void OnNavBottomLeft(InputAction.CallbackContext context)
    {
        OnNavBottomLeftEvent.Invoke();
    }

    private void OnNavBottomRight(InputAction.CallbackContext context)
    {
        OnNavBottomRightEvent.Invoke();
    }

    private void OnNavMiddleLeft(InputAction.CallbackContext context)
    {
        OnNavMiddleLeftEvent.Invoke();
    }

    private void OnNavMiddleRight(InputAction.CallbackContext context)
    {
        OnNavMiddleRightEvent.Invoke();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }
}