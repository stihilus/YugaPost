using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class UIInventory : MonoBehaviour
{
    private InputEventManager inputEventManager;
    [SerializeField] MailBehaviour mailBehaviour;
    [SerializeField] StarterAssets.ThirdPersonController player;
    public GameObject menuPanel;
    public GameObject inventoryPanel;
    public GameObject mapPanel;
    public GameObject photoPanel;

    public Image fader;
    public GameObject renderTexture;

    [Header("General")]
    public TextMeshProUGUI cashText;

    [Header("Inventory")]
    public TextMeshProUGUI recieverName;
    public TextMeshProUGUI address;
    public TextMeshProUGUI number;
    public Button next;
    public Button previous;
    private int currentMailIndex;

    private void Start()
    {
        cashText.text = GameManager.Instance.cash.ToString();
        
        cashText.text = "$ " + GameManager.Instance.cash.ToString();
        
        // Add all listeners
        GameEvents.OnCashChanged += OnCashChanged;
        
        GameEvents.OnPhotoModeOn += DisableRender;
        GameEvents.OnPhotoModeOff += EnableRender;

        inputEventManager = InputEventManager.Instance;
        
        inputEventManager.OnOpenUIEvent.AddListener(ToggleMenu);
        inputEventManager.OnMapEvent.AddListener(OpenMap);
    }

    private void OnCashChanged(int obj)
    {
        cashText.text = "$ " + obj.ToString();
    }

    private void DisableRender()
    {
        renderTexture.SetActive(false);
    }

    private void EnableRender()
    {
        renderTexture.SetActive(true);
    }

    private void ToggleMenu()
    {
        menuPanel.SetActive(!menuPanel.activeSelf);

        OpenInventory();

        if (menuPanel.activeSelf)
        {
            player.animator.SetBool("FreeFall", false);
            player.animator.SetBool("Jump", false);
            player.animator.SetBool("Grounded", true);
            player.animator.SetFloat("Speed", 0);
            player.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            player.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void OpenInventory()
    {
        InitInventory();
        inventoryPanel.SetActive(true);
        photoPanel.SetActive(false);
        mapPanel.SetActive(false);
    }

    public void OpenMap()
    {
        if(!menuPanel.activeSelf)
        {
            ToggleMenu();
            inventoryPanel.SetActive(false);
            photoPanel.SetActive(false);
            mapPanel.SetActive(true);
        }
        else
        {
            inventoryPanel.SetActive(false);
            photoPanel.SetActive(false);
            mapPanel.SetActive(true);
        }
    }

    public void OpenPhoto()
    {
        if (!menuPanel.activeSelf)
        {
            ToggleMenu();
            inventoryPanel.SetActive(false);
            mapPanel.SetActive(false);
            photoPanel.SetActive(true);
        }
        else
        {
            inventoryPanel.SetActive(false);
            mapPanel.SetActive(false);
            photoPanel.SetActive(true);
        }
    }

    public void InitInventory()
    {
        currentMailIndex = 0;

        UpdateInventory();
    }

    public void UpdateInventory()
    {
        if (mailBehaviour.mails.Count > 0)
        {
            recieverName.text = mailBehaviour.mails[currentMailIndex].receiver;
            address.text = mailBehaviour.mails[currentMailIndex].address;
            number.text = mailBehaviour.mails[currentMailIndex].number.ToString();
            next.interactable = true;
            previous.interactable = true;
        }
        else
        {
            recieverName.text = "No mail";
            address.text = "X";
            number.text = "X";
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

        UpdateInventory();
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

        UpdateInventory();
    }

    private void OnDestroy()
    {
        if (inputEventManager == null)
            return;
            
        inputEventManager.OnOpenUIEvent.RemoveListener(ToggleMenu);
        inputEventManager.OnMapEvent.RemoveListener(OpenMap);

        // Remove all listeners
        GameEvents.OnCashChanged -= OnCashChanged;

        GameEvents.OnPhotoModeOff -= EnableRender;
        GameEvents.OnPhotoModeOn -= DisableRender;
    }
}
