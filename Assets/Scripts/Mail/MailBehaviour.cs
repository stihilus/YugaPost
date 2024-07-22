using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailBehaviour : MonoBehaviour
{
    [HideInInspector] public List<Mail> mails;
    public MailGenerator postOffice;
    private Mailbox currentMailbox;
    [SerializeField] private PlayerUI playerUI;

    private int mailCashReward = 100;

    [Header("Package")]
    [SerializeField] private Package package;
    [SerializeField] private Transform[] packageSpawns;
    
    private void Awake() {
        mails = new List<Mail>();
    }

    private void Start()
    {
        PackageSpawner();
    }

    private void PackageSpawner()
    {
        foreach (Transform spawn in packageSpawns)
        {
            Package newPackage = Instantiate(package, spawn.position, Quaternion.identity);
            newPackage.Init();
        }
    }

    private Mail PickUpMail()
    {
        Mail newMail = postOffice.GenerateMail();
        mails.Add(newMail);
        playerUI.SetGeneralText("Mail picked up!");
        playerUI.FadeIn();

        return newMail;
    }

    private void DropMail()
    {
        bool mailDelivered = false;
        bool mailFound = false;

        foreach (Mail mail in mails)
        {
            if (mail.address == currentMailbox.address && mail.number == currentMailbox.number)
            {
                mails.Remove(mail);
                playerUI.SetGeneralText("Mail delivered to " + currentMailbox.address + " " + currentMailbox.number + " from " + mail.sender + " to " + mail.receiver);
                playerUI.FadeIn();
                mailDelivered = true;
                GameEvents.MailDelivered();
                GameEvents.CashChanged(GameManager.Instance.cash += mailCashReward);
                break;
            }
            else if (mail.address == currentMailbox.address)
            {
                mailFound = true;
            }
        }

        if (!mailDelivered)
        {
            if (mailFound)
            {
                playerUI.SetGeneralText("No mail with the correct number for " + currentMailbox.address + "...");
            }
            else
            {
                playerUI.SetGeneralText("No mail for " + currentMailbox.address + " " + currentMailbox.number + "...");
            }

            playerUI.FadeIn();
        }

        if (mails.Count == 0)
        {
            GameEvents.AllMailDelivered();
            PackageSpawner();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("PostOffice"))
        // {
        //     InputEventManager.Instance.OnInteractEvent.AddListener(PickUpMail);
        // }

        if (other.CompareTag("Package"))
        {
            PickUpMail();
            other.GetComponent<Package>().TakePackage();
        }

        if (other.CompareTag("Mailbox"))
        {
            currentMailbox = other.GetComponent<Mailbox>();
            currentMailbox.FadeIn();
            InputEventManager.Instance.OnInteractEvent.AddListener(DropMail);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag("PostOffice"))
        // {
        //     InputEventManager.Instance.OnInteractEvent.RemoveListener(PickUpMail);
        // }

        if (other.CompareTag("Mailbox"))
        {
            currentMailbox.FadeOut();
            currentMailbox = null;
            InputEventManager.Instance.OnInteractEvent.RemoveListener(DropMail);
        }
    }
}
