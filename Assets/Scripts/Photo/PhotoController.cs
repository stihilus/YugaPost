using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class PhotoController : MonoBehaviour
{
    public static PhotoController Instance { get; private set; }
    
    public InputActionReference photoCamera;
    public InputActionReference takePhoto;
    public InputActionReference photoZoom;

    public GameObject mainCameraObject;
    public Camera photoCameraObject;
    public GameObject photoRenderTexture;
    public float zoomAmount;

    public Image blackFader;

    public bool photoMode = false;

    [Header("General")]
    public int photoShotsRemaining;
    public int photoShotsMax = 10;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shutterSound;
    public AudioClip zoomInSound;
    public AudioClip zoomOutSound;
    public AudioClip photoModeOnSound;
    public AudioClip photoModeOffSound;

    [Header("Photo Album")]
    public PhotoAlbum photoAlbum;
    public List<Image> photoAlbumImages;
    
    private void OnEnable() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        GameEvents.OnPhotoShotsPurchased += OnPhotoShotsPurchased;
        
        photoCamera.action.performed += OnPhotoCamera;

        takePhoto.action.performed += OnTakePhoto;

        photoZoom.action.performed += OnPhotoZoom;

        GameEvents.OnPhotosCollected += PopulateAlbum;
    }

    // make photo camera match the main camera rotation
    private void Update() 
    {
        if (!photoMode) return;
        
        photoCameraObject.transform.rotation = mainCameraObject.transform.rotation;
    }

    public static int GetPhotoShotsRemaining()
    {
        return Instance.photoShotsRemaining;
    }

    public static int SetPhotoShotsRemaining(int value)
    {
        return Instance.photoShotsRemaining = value;
    }

    private void OnPhotoShotsPurchased()
    {
        photoShotsRemaining = photoShotsMax;
    }

    private void OnPhotoZoom(InputAction.CallbackContext context)
    {
        if (!context.performed || !photoMode) return;

        // Read the normalized value, adjust it to control the speed of zoom
        float zoomDelta = Mathf.Clamp(-context.ReadValue<float>(), -zoomAmount, zoomAmount);

        // Calculate the target field of view
        float targetFOV = photoCameraObject.fieldOfView + zoomDelta;

        // Ensure the targetFOV is within a reasonable range
        targetFOV = Mathf.Clamp(targetFOV, 10f, 60f);

        // Tween the camera FOV to the target value
        photoCameraObject.DOFieldOfView(targetFOV, 0.5f);

        // Play the zoom sound
        if (zoomDelta > 0)
        {
            AudioControl.Play(audioSource, zoomInSound);
        }
        else if (zoomDelta < 0)
        {
            AudioControl.Play(audioSource, zoomOutSound);
        }
    }

    // toggle photo mode
    public void OnPhotoCamera(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        photoMode = !photoMode;

        if (photoMode)
        {
            GameEvents.PhotoModeOn();

            AudioControl.Play(audioSource, photoModeOnSound);

            photoRenderTexture.SetActive(true);
        }
        else if (!photoMode)
        {
            GameEvents.PhotoModeOff();

            AudioControl.Play(audioSource, photoModeOffSound);

            photoRenderTexture.SetActive(false);
        }
    }

    // take photo
    public void OnTakePhoto(InputAction.CallbackContext context)
    {
        if (!context.performed || !photoMode) return;
        
        if (photoShotsRemaining <= 0)
        {
            Debug.Log("No photo shots remaining");
            return;
        }

        photoShotsRemaining--;

        GameManager.Instance.photoCounter++;
        
        GameEvents.TakePhoto();

        AudioControl.Play(audioSource, shutterSound);

        // Fade the screen to black and back to white
        blackFader.DOFade(1f, 0.1f).OnComplete(() => blackFader.DOFade(0f, 0.1f));

        // Start a coroutine to capture the screen after rendering
        StartCoroutine(CaptureScreen(photoCameraObject, LayerMask.NameToLayer("UI")));
    }

    // private IEnumerator CaptureScreen()
    // {
    //     // Wait for the end of the current frame
    //     yield return new WaitForEndOfFrame();

    //     // Create a new texture with the width and height of the screen
    //     Texture2D photo = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

    //     // Read the pixels from the screen into the texture
    //     photo.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

    //     // Apply the changes to the texture
    //     photo.Apply();

    //     // Convert the texture to a sprite
    //     Sprite photoSprite = Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f));

    //     // Generate a unique filename based on the current timestamp
    //     string fileName = "YugaPostPhoto_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

    //     // Save the texture to a file
    //     SaveLoad.SaveTexture(fileName, photo);

    //     // // Add the sprite to the photo album
    //     // photoAlbum.AddPhoto(photoSprite);

    //     // // Set the next image in the list as the photo sprite
    //     // photoAlbumImages[photoAlbum.photos.Count - 1].sprite = photoSprite;
    // }

    private IEnumerator CaptureScreen(Camera camera, int layerToIgnore)
    {
        // Wait for the end of the current frame
        yield return new WaitForEndOfFrame();

        // Create a new RenderTexture with the same dimensions as the screen
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camera.targetTexture = renderTexture;

        // Set the cullingMask to exclude the layer you want to ignore
        camera.cullingMask = ~(1 << layerToIgnore);

        // Render the scene
        camera.Render();

        // Create a new Texture2D and read the pixels from the RenderTexture
        Texture2D photo = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        photo.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        photo.Apply();

        // Convert the texture to a sprite
        Sprite photoSprite = Sprite.Create(photo, new Rect(0, 0, photo.width, photo.height), new Vector2(0.5f, 0.5f));

        // Generate a unique filename based on the current timestamp
        string fileName = "YugaPostPhoto_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

        // Save the texture to a file
        SaveLoad.SaveTexture(fileName, photo);

        // Add the sprite to the photo album
        photoAlbum.AddPhoto(photoSprite);

        // Reset the camera's targetTexture and cullingMask
        camera.targetTexture = photoRenderTexture.GetComponent<RawImage>().texture as RenderTexture;
        camera.cullingMask = ~0;
    }

    private void PopulateAlbum()
    {
        for (int i = 0; i < photoAlbum.photos.Count; i++)
        {
            photoAlbumImages[i].sprite = photoAlbum.photos[i];
        }
    }

    private void OnDisable() 
    {
        GameEvents.OnPhotoShotsPurchased -= OnPhotoShotsPurchased;
        
        photoCamera.action.performed -= OnPhotoCamera;

        takePhoto.action.performed -= OnTakePhoto;

        photoZoom.action.performed -= OnPhotoZoom;

        GameEvents.OnPhotosCollected -= PopulateAlbum;
    }
}
