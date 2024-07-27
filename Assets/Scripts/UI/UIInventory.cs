using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.IO;

public enum UIDirections
{
    Left,
    Right
}

public class UIInventory : MonoBehaviour
{
    private InputEventManager inputEventManager;
    [SerializeField] MailBehaviour mailBehaviour;
    [SerializeField] StarterAssets.ThirdPersonController player;
    public GameObject menuPanel;

    public Image fader;
    public GameObject renderTexture;

    [Header("General")]
    public TextMeshProUGUI cashText;
    public Button tabButton;
    public GameObject navMiddle;

    [Header("Shipments")]
    public Sprite[] postmarks;
    public TextMeshProUGUI recieverName;
    public TextMeshProUGUI address;
    public TextMeshProUGUI number;
    public Image postmark1;
    public Image postmark2;
    public Button next;
    public Button previous;
    private int currentMailIndex;

    [Header("Photos")]
    public List<Sprite> photos;
    public PhotoAlbum photoAlbum;
    public Image photo;

    public float middleElementsCycleSpeed;
    public GameObject[] tabBtns;
    public GameObject[] tabs;
    private int currentTabIndex = 0;

    private void Start()
    {
        cashText.text = GameManager.Instance.cash.ToString();
        
        cashText.text = "$ " + GameManager.Instance.cash.ToString();
        
        // Add all listeners
        GameEvents.OnCashChanged += OnCashChanged;
        
        GameEvents.OnPhotoModeOn += DisableRender;
        GameEvents.OnPhotoModeOn += HideUI;
        GameEvents.OnPhotoModeOff += EnableRender;
        GameEvents.OnPhotoModeOff += ShowUI;

        inputEventManager = InputEventManager.Instance;

        inputEventManager.OnOpenUIEvent.AddListener(ToggleMenu);
        inputEventManager.OnCloseUIEvent.AddListener(ToggleMenu);

        inputEventManager.inputActions.UI.Disable();

        InitShipments();
        InitPhotos();
    }

    private void CycleTabs(UIDirections direction)
    {
        if (direction == UIDirections.Left)
        {
            currentTabIndex = (currentTabIndex - 1 + tabs.Length) % tabs.Length;
        }
        else
        {
            currentTabIndex = (currentTabIndex + 1) % tabs.Length;
        }

        if (currentTabIndex == 1)
        {
            navMiddle.SetActive(false);
        }
        else
        {
            navMiddle.SetActive(true);
        }

        UpdateTabVisibility();
    }

    // this method moves the middle element to the left or right, fading it it's canvas group, and then moves that same element
    // to the opposite side of the list, fading it back in and changing the content based on if it's mail or photo
    private void CycleMiddleElements(UIDirections direction)
    {
        if (currentTabIndex == 0 && mailBehaviour.mails.Count <= 1) return;

        if (currentTabIndex == 1) return;

        if (currentTabIndex == 2 && GameManager.Instance.photoCounter == 0) return;
        
        if (direction == UIDirections.Left)
        {
            inputEventManager.inputActions.Disable();
            // fade out the middle element
            tabs[currentTabIndex].GetComponent<CanvasGroup>().DOFade(0, middleElementsCycleSpeed).OnComplete(() =>
            {
                inputEventManager.inputActions.Enable();
                
                // fade in the middle element
                tabs[currentTabIndex].GetComponent<CanvasGroup>().DOFade(1, middleElementsCycleSpeed);
                if(currentTabIndex == 0)
                {
                    PreviousMail();
                }
                else if(currentTabIndex == 2)
                {
                    PreviousPhoto();
                }
            });
        }
        else
        {
            inputEventManager.inputActions.Disable();
            // fade out the middle element
            tabs[currentTabIndex].GetComponent<CanvasGroup>().DOFade(0, middleElementsCycleSpeed).OnComplete(() =>
            {
                inputEventManager.inputActions.Enable();
                
                // fade in the middle element
                tabs[currentTabIndex].GetComponent<CanvasGroup>().DOFade(1, middleElementsCycleSpeed);
                if(currentTabIndex == 0)
                {
                    NextMail();
                }
                else if(currentTabIndex == 2)
                {
                    NextPhoto();
                }
            });
        }
    }

    private void UpdateTabVisibility()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].SetActive(i == currentTabIndex);

            // animate the tab to move up 5 units
            if (i == currentTabIndex)
            {
                tabBtns[i].transform.DOLocalMoveY(5, 0.5f);
            }
            else
            {
                tabBtns[i].transform.DOLocalMoveY(0, 0.5f);
            }
        }
    }

    private void OnCashChanged(int obj)
    {
        cashText.text = "$ " + obj.ToString();
    }

    private void EnableRender()
    {
        renderTexture.SetActive(true);
    }

    private void DisableRender()
    {
        renderTexture.SetActive(false);
    }

    private void ShowUI()
    {
        cashText.gameObject.SetActive(true);
        tabButton.gameObject.SetActive(true);
    }

    private void HideUI()
    {
        cashText.gameObject.SetActive(false);
        tabButton.gameObject.SetActive(false);
    }

    private void ToggleMenu()
    {
        if (GameStateManager.CurrentGameState == GameState.Photo)
            return;

        menuPanel.SetActive(!menuPanel.activeSelf);

        if (menuPanel.activeSelf)
        {
            inputEventManager.inputActions.Player.Disable();
            inputEventManager.inputActions.UI.Enable();
            
            player.animator.SetBool("FreeFall", false);
            player.animator.SetBool("Jump", false);
            player.animator.SetBool("Grounded", true);
            player.animator.SetFloat("Speed", 0);
            player.enabled = false;
            HideUI();
            
            UpdateTabVisibility();
            UpdateShipments();
            UpdatePhotoAlbumFromPersistentDataPath();

            navMiddle.SetActive(true);
            
            inputEventManager.OnNavBottomLeftEvent.AddListener(() => CycleTabs(UIDirections.Left));
            inputEventManager.OnNavBottomRightEvent.AddListener(() => CycleTabs(UIDirections.Right));

            inputEventManager.OnNavMiddleLeftEvent.AddListener(() => CycleMiddleElements(UIDirections.Left));
            inputEventManager.OnNavMiddleRightEvent.AddListener(() => CycleMiddleElements(UIDirections.Right));

            GameStateManager.SetGameState(GameState.Inventory);
        }
        else
        {
            InputEventManager.Instance.inputActions.Player.Enable();
            InputEventManager.Instance.inputActions.UI.Disable();
            
            player.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            ShowUI();
            
            inputEventManager.OnNavBottomLeftEvent.RemoveAllListeners();
            inputEventManager.OnNavBottomRightEvent.RemoveAllListeners();

            inputEventManager.OnNavMiddleLeftEvent.RemoveAllListeners();
            inputEventManager.OnNavMiddleRightEvent.RemoveAllListeners();

            currentTabIndex = 0;

            GameStateManager.SetGameState(GameState.InGame);
        }
    }

    public void InitShipments()
    {
        currentMailIndex = 0;

        UpdateShipments();
    }

    public void UpdateShipments()
    {
        if (mailBehaviour.mails.Count > 0)
        {
            recieverName.text = mailBehaviour.mails[currentMailIndex].receiver;
            address.text = mailBehaviour.mails[currentMailIndex].address;
            number.text = mailBehaviour.mails[currentMailIndex].number.ToString();
            postmark1.sprite = postmarks[mailBehaviour.mails[currentMailIndex].postmark1];
            postmark2.sprite = postmarks[mailBehaviour.mails[currentMailIndex].postmark2];
            postmark1.SetNativeSize();
            postmark2.SetNativeSize();
            postmark1.gameObject.SetActive(true);
            postmark2.gameObject.SetActive(true);

            next.interactable = true;
            previous.interactable = true;
        }
        else
        {
            recieverName.text = "No mail";
            address.text = "";
            number.text = "";
            postmark1.gameObject.SetActive(false);
            postmark2.gameObject.SetActive(false);
            
            next.interactable = false;
            previous.interactable = false;
        }
    }

    public void NextMail()
    {
        if (currentMailIndex < mailBehaviour.mails.Count - 1)
        {
            currentMailIndex++;
        }
        else
        {
            currentMailIndex = 0;
        }

        UpdateShipments();
    }

    public void PreviousMail()
    {
        if (currentMailIndex > 0)
        {
            currentMailIndex--;
        }
        else
        {
            currentMailIndex = mailBehaviour.mails.Count - 1;
        }

        UpdateShipments();
    }

#region Photos
    public void InitPhotos()
    {
        UpdatePhotoAlbumFromPersistentDataPath();

        if (photos.Count > 0)
        {
            photo.color = Color.white;
            photo.sprite = photos[0];
        }
        else
        {
            photo.color = Color.black;
        }
    }

    public void UpdatePhotoAlbumFromPersistentDataPath()
    {
        // load all photos from the persistent data path
        string[] photoFileNames = Directory.GetFiles(Application.persistentDataPath, "*.png");

        foreach (string fileName in photoFileNames)
        {
            string fileNameWithoutPath = Path.GetFileName(fileName);
            
            if (photoAlbum.photos.Contains(fileNameWithoutPath)) continue;
            
            photoAlbum.AddPhoto(fileNameWithoutPath);
        }

        List<string> photosToRemove = new List<string>();

        foreach (string photo in photoAlbum.photos)
        {
            bool photoExists = false;
            foreach (string fileName in photoFileNames)
            {
                if (Path.GetFileName(fileName) == photo)
                {
                    photoExists = true;
                    break;
                }
            }

            if (!photoExists)
            {
                photosToRemove.Add(photo);
            }
        }

        // Remove the collected photos
        foreach (string photo in photosToRemove)
        {
            photoAlbum.RemovePhoto(photo);
        }

        UpdatePhotos();
    }

    public void UpdatePhotos()
    {
        // load all photos from the persistent data path

        photos.Clear();

        foreach (string fileName in photoAlbum.photos)
        {
            Texture2D texture = SaveLoad.LoadTexture(fileName);

            if (texture != null)
            {
                // Convert the texture to a sprite
                Sprite photoSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));
                photoSprite.name = fileName;
                photos.Add(photoSprite);
            }
        }

        if (photos.Count > 0)
        {
            photo.color = Color.white;
            photo.sprite = photos[0];
        }
        else
        {
            photo.color = Color.black;
        }
    }

    public void NextPhoto()
    {
        photo.sprite = photos[(photos.IndexOf(photo.sprite) + 1) % photos.Count];
    }

    public void PreviousPhoto()
    {
        photo.sprite = photos[(photos.IndexOf(photo.sprite) - 1 + photos.Count) % photos.Count];
    }

#endregion

    private void OnDestroy()
    {
        if (inputEventManager == null)
            return;
            
        inputEventManager.OnOpenUIEvent.RemoveListener(ToggleMenu);

        // Remove all listeners
        GameEvents.OnCashChanged -= OnCashChanged;

        GameEvents.OnPhotoModeOn -= DisableRender;;
        GameEvents.OnPhotoModeOn -= HideUI;
        GameEvents.OnPhotoModeOff -= EnableRender;
        GameEvents.OnPhotoModeOff -= ShowUI;
    }
}
