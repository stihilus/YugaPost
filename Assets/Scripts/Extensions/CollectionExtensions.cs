using System;
using System.Collections.Generic;
using UnityEngine;

public static class CollectionExtensions
{
    public static T GetRandom<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            throw new InvalidOperationException("The list is null or empty.");
        }

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static T GetRandom<T>(this T[] array)
    {
        if (array == null || array.Length == 0)
        {
            throw new InvalidOperationException("The array is null or empty.");
        }

        int randomIndex = UnityEngine.Random.Range(0, array.Length);
        return array[randomIndex];
    }
}
