using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class MapManager
{
    public static List<MapLocation> map;
    public static float[] mapUnitXYScale = new float[2];

    public static Unit player;
    public static Button actionBatton;
    public static Text actionButtonText;
    public static AbstractPlayerActionPointer playerAction;

    public static List<MapLocation> directions = new List<MapLocation>() { new MapLocation(-1, 0), new MapLocation(1, 0), new MapLocation(0, 1), new MapLocation(0, -1),
                                                                    new MapLocation(-1, 1), new MapLocation(1, 1), new MapLocation(-1, -1), new MapLocation(1, -1)};
    public static bool IsHasNeighbours(Vector3 position, Vector3 pivot)
    {
        bool hasNeighbours = true;
        
        if (position.x != pivot.x && position.z != pivot.z)
        {
            if (!MapManager.map.Exists(x => x.Equals(new MapLocation(position.x, pivot.z))) || !MapManager.map.Exists(x => x.Equals(new MapLocation(pivot.x, position.z))))
            {
                hasNeighbours = false;
            }
        }

        return hasNeighbours;
    }

    public static bool WillEnemyMeet(Vector3 position, Vector3 pivot)
    {
        bool willMeet = false;

        MapLocation firstLocation = MapManager.map.Find(x => x.Equals(new MapLocation(position.x, pivot.z)));
        MapLocation secondLocation = MapManager.map.Find(x => x.Equals(new MapLocation(pivot.x, position.z)));

        if (position.x != pivot.x && position.z != pivot.z)
        {
            if ((firstLocation.isclosed && secondLocation.willBeClosed) || (secondLocation.isclosed && firstLocation.willBeClosed))
            {
                willMeet = true;
            }
        }

        return willMeet;
    }

    public static void SetMapFromTilemapTransform(Transform tilemap)
    {
        map = new List<MapLocation>();
        for (int i = 0; i < tilemap.childCount; i++)
        {
            map.Add(new MapLocation(tilemap.GetChild(i).position.x, tilemap.GetChild(i).position.z));
        }

        map = map.OrderBy(p => p.x).ThenBy(p =>p.z).ToList<MapLocation>();

        SetMapUnitScale(tilemap);
    }

    private static void SetMapUnitScale(Transform tilemap)
    {
        mapUnitXYScale[0] = tilemap.parent.GetComponent<Grid>().cellSize.x;
        mapUnitXYScale[1] = tilemap.parent.GetComponent<Grid>().cellSize.y;
    }

}
