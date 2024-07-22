using UnityEngine;
using System;

public static class GameEvents
{
    // Fade
    public static event Action OnFadeIn;
    public static event Action OnFadeOut;
    
    // Photo
    public static event Action OnPhotoShotsPurchased;
    public static event Action OnPhotoModeOn;
    public static event Action OnPhotoModeOff;
    public static event Action OnTakePhoto;
    public static event Action OnPhotosBeingDeveloped;
    public static event Action OnPhotosDeveloped;
    public static event Action OnPhotosCollected;

    // Mail
    public static event Action OnMailDelivered;
    public static event Action OnAllMailDelivered;

    // Currency
    public static event Action<int> OnCashChanged;

    // Day end
    public static event Action OnDayEnded;

    /// <summary>
    /// Fade in the screen from black to white
    /// </summary>
    public static void FadeIn()
    {
        OnFadeIn?.Invoke();
    }

    /// <summary>
    /// Fade out the screen from white to black
    /// </summary>
    public static void FadeOut()
    {
        OnFadeOut?.Invoke();
    }

    public static void PhotoShotsPurchased()
    {
        Debug.Log("Photo shots purchased");
        OnPhotoShotsPurchased?.Invoke();
    }

    public static void PhotoModeOn()
    {
        Debug.Log("Photo mode enabled");
        OnPhotoModeOn?.Invoke();
    }

    public static void PhotoModeOff()
    {
        Debug.Log("Photo mode disabled");
        OnPhotoModeOff?.Invoke();
    }

    public static void TakePhoto()
    {
        Debug.Log("Photo taken");
        OnTakePhoto?.Invoke();
    }

    public static void PhotosBeingDeveloped()
    {
        Debug.Log("Photos being developed");
        OnPhotosBeingDeveloped?.Invoke();
    }

    public static void PhotosDeveloped()
    {
        Debug.Log("Photos developed");
        OnPhotosDeveloped?.Invoke();
    }

    public static void PhotosCollected()
    {
        Debug.Log("Photos collected");
        OnPhotosCollected?.Invoke();
    }

    public static void MailDelivered()
    {
        Debug.Log("Mail delivered");
        OnMailDelivered?.Invoke();
    }

    public static void AllMailDelivered()
    {
        Debug.Log("All mail delivered");
        OnAllMailDelivered?.Invoke();
    }

    public static void CashChanged(int cash)
    {
        Debug.Log("Cash changed");
        OnCashChanged?.Invoke(cash);
    }

    public static void DayEnded()
    {
        Debug.Log("Day ended");
        OnDayEnded?.Invoke();
    }
}