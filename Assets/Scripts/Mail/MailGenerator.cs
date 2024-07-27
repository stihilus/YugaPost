using System.Collections.Generic;
using UnityEngine;

public class MailGenerator : MonoBehaviour
{
    private MailData mailData;

    private void Awake()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("MailData");

        if (jsonFile == null)
        {
            Debug.LogError("Failed to load MailData JSON file.");
            return;
        }

        try
        {
            mailData = JsonUtility.FromJson<MailData>(jsonFile.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error during deserialization: " + ex.Message);
            return;
        }

        if (mailData == null)
        {
            Debug.LogError("Failed to parse MailData from JSON.");
            return;
        }
        
        mailData = JsonUtility.FromJson<MailData>(Resources.Load<TextAsset>("MailData").text);
    }

    public Mail GenerateMail()
    {
        if (mailData == null || mailData.addresses == null || mailData.addresses.Count == 0)
        {
            Debug.LogError("MailData or addresses list is null or empty.");
            return null; // or handle the situation accordingly
        }

        Mail mail = new Mail();

        mail.sender = mailData.names.GetRandom() + " " + mailData.surnames.GetRandom();
        mail.receiver = mailData.names.GetRandom() + " " + mailData.surnames.GetRandom();

        AddressData addressData = mailData.addresses.GetRandom();

        mail.address = addressData.street;
        mail.number = addressData.numbers.GetRandom();

        mail.postmark1 = mailData.postmarks.GetRandom();
        mail.postmark2 = mailData.postmarks.GetRandom();

        mail.message = mailData.intros.GetRandom() + "/n/n" 
            + mailData.middles.GetRandom() + " "
            + mailData.concepts.GetRandom() + "/n/n"
            + mailData.finishings.GetRandom() + "/n/n"
            + mail.sender;

        Debug.Log("Mail generated: " + mail.sender + " to " + mail.receiver + " at " + mail.address + " " + mail.number);

        return mail;
    }
}
