using UnityEngine;
using System.IO;

public static class SaveLoad
{
    // Save simple data
    public static void SaveData(string fileName, SaveData data)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    // Load simple data
    public static SaveData LoadData(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            // Handle the case where the save file doesn't exist
            return new SaveData();
        }
    }

    // Save texture
    public static void SaveTexture(string fileName, Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllBytes(filePath, bytes);
    }

    // Load texture
    public static Texture2D LoadTexture(string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            byte[] bytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2); // Set dimensions appropriately
            texture.LoadImage(bytes);
            return texture;
        }
        else
        {
            // Handle the case where the texture file doesn't exist
            return null;
        }
    }
}
