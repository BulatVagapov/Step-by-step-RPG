using System.Collections.Generic;
using UnityEngine;

public static class PlayerOutlineActionPointersManager
{
    public static List<AbstractPlayerActionPointer> playerActions = new List<AbstractPlayerActionPointer>();

    public static void ShowPlayerActionPointers()
    {
        if (playerActions.Count > 0)
        {
            foreach (AbstractPlayerActionPointer action in playerActions)
            {
                if (Mathf.Abs(Vector3.Distance(action.transform.position, MapManager.player.transform.position)) < 1.5f * MapManager.mapUnitXYScale[0])
                {
                    if(MapManager.IsHasNeighbours(action.transform.position, MapManager.player.transform.position))
                        action.enabled = true;
                }
            }
        }
    }

    public static void HideplayerActionPointers()
    {
        if (playerActions.Count > 0)
        {
            foreach (AbstractPlayerActionPointer action in playerActions)
            {
                if (action.enabled)
                {
                    action.enabled = false;
                }
            }
        }
    }
}
