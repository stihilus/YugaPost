using UnityEngine;
using TMPro;
using DG.Tweening;

public class PhotoKiosk : MonoBehaviour, IWorldInteractable
{
    public TextMeshProUGUI infoText;
    public int photoShotsPrice;
    public int developPhotosPrice;
    
    private bool photosAreBeingDeveloped = false;
    private bool hasDevelopedPhotos = false;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        infoText.text = "Purchase photo shots for $100 ?";
        infoText.DOFade(0, 0);

        GameEvents.OnPhotosBeingDeveloped += PhotosBeingDeveloped;
        GameEvents.OnDayEnded += FinishPhotosBeingDeveloped;
    }

    private void PhotosBeingDeveloped()
    {
        photosAreBeingDeveloped = true;
    }

    public void FadeIn()
    {
        infoText.DOFade(1, 0.2f);
    }

    public void FadeOut()
    {
        infoText.DOFade(0, 0.2f);
    }

    private void InteractWithKiosk()
    {
        if (hasDevelopedPhotos)
            CollectPhotos();
        else if (GameManager.Instance.photoCounter == 0 && !photosAreBeingDeveloped)
            PurchasePhotoShots();
        else if (GameManager.Instance.photoCounter > 0)
            DevelopPhotos();
    }

    private void PurchasePhotoShots()
    {
        if (photosAreBeingDeveloped)
        {
            infoText.text = "Photos are being developed, come back tomorrow!";
            return;
        }
        
        if (GameManager.Instance.cash < photoShotsPrice && PhotoController.GetPhotoShotsRemaining() == 0)
        {
            infoText.text = "Not enough cash!";
            return;
        }

        if (PhotoController.GetPhotoShotsRemaining() > 0)
        {
            infoText.text = "You still have photos to shoot!";
            return;
        }
        
        GameEvents.PhotoShotsPurchased();
        GameEvents.CashChanged(GameManager.Instance.cash -= photoShotsPrice);

        infoText.text = "Photo shots purchased!";
    }

    private void DevelopPhotos()
    {
        if (GameManager.Instance.cash < developPhotosPrice)
        {
            infoText.text = "Not enough cash!";
            return;
        }
        
        GameManager.Instance.photoCounter = 0;
        PhotoController.SetPhotoShotsRemaining(0);

        infoText.text = "Photos being developed, come back tomorrow!";
        GameEvents.PhotosBeingDeveloped();
    }

    private void CollectPhotos()
    {
        if (!hasDevelopedPhotos)
        {
            infoText.text = "No photos to collect!";
            return;
        }

        hasDevelopedPhotos = false;
        infoText.text = "Photos collected!";

        GameEvents.PhotosCollected();
    }

    private void FinishPhotosBeingDeveloped()
    {
        photosAreBeingDeveloped = false;
        hasDevelopedPhotos = true;

        GameEvents.PhotosDeveloped();
    }

    private void MessageOnEnter()
    {
        if (photosAreBeingDeveloped)
        {
            infoText.text = "Photos are being developed, come back tomorrow!";
            return;
        }

        if (hasDevelopedPhotos)
        {
            infoText.text = "Collect photos?";
            return;
        }
        
        if(GameManager.Instance.photoCounter > 0)
            infoText.text = "Develop photos for $ " + developPhotosPrice + "?";
        else if(GameManager.Instance.photoCounter == 0 && PhotoController.GetPhotoShotsRemaining() == 0)
            infoText.text = "Purchase photo shots for $ " + photoShotsPrice + "?";
        else if(GameManager.Instance.photoCounter == 0 && PhotoController.GetPhotoShotsRemaining() > 0)
            infoText.text = "You still have photos to shoot!";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MessageOnEnter();
            FadeIn();
            InputEventManager.Instance.OnInteractEvent.AddListener(InteractWithKiosk);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputEventManager.Instance.OnInteractEvent.RemoveListener(InteractWithKiosk);
            FadeOut();
        }
    }

    private void OnDestroy()
    {
        GameEvents.OnPhotosBeingDeveloped -= PhotosBeingDeveloped;
        GameEvents.OnDayEnded -= FinishPhotosBeingDeveloped;
    }
}
