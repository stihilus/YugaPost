using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhotoAlbum", menuName = "ScriptableObjects/PhotoAlbum", order = 1)]
public class PhotoAlbum : ScriptableObject
{
    public List<Sprite> photos = new List<Sprite>();

    public void AddPhoto(Sprite photo)
    {
        photos.Add(photo);
    }

    public void RemovePhoto(Sprite photo)
    {
        photos.Remove(photo);
    }
}
