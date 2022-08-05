using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Utilities
{
    public static Bounds GetBounds<T>(IEnumerable<T> gameObjects) where T : MonoBehaviour
    {
        var gameObjectsList = gameObjects.ToList();
        var totalBounds = gameObjectsList[0].GetBounds();

        if (gameObjectsList.Count > 1)
        {
            for (int i = 1; i < gameObjectsList.Count; i++)
                totalBounds.Encapsulate(gameObjectsList[i].GetBounds());
        }

        return totalBounds;
    }
}