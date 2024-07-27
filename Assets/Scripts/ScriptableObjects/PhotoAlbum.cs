using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhotoAlbum", menuName = "ScriptableObjects/PhotoAlbum", order = 1)]
public class PhotoAlbum : ScriptableObject
{
    public List<string> photos = new List<string>();

    public void AddPhoto(string fileName)
    {
        photos.Add(fileName);
    }

    public void RemovePhoto(string fileName)
    {
        photos.Remove(fileName);
    }
}
