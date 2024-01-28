using UnityEngine;

public class MapLocation
{
    public float x;
    public float z;
    public bool isclosed;
    public bool willBeClosed;

    public MapLocation(float x, float z)
    {
        this.x = x;
        this.z = z;
    }

    public bool Equals(MapLocation location)
    {
        return x == location.x && z == location.z;
    }

    public bool Equals(Vector3 location)
    {
        return x == location.x && z == location.z;
    }

    public static MapLocation operator + (MapLocation a, MapLocation b)
    {
        return new MapLocation((a.x + b.x), (a.z + b.z));
    }

    public Vector3 ToVector()
    {
        return new Vector3(x, 0, z);
    }
}
