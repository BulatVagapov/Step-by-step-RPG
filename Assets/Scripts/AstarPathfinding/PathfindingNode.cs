using UnityEngine;

public class PathfindingNode
{
    public float G { get; set; }
    public float H { get; set; }
    public float F { get; set; }
    public PathfindingNode Parent { get; set; }
    public MapLocation Position { get; set; }

    public PathfindingNode(MapLocation location, float g, float h, float f, PathfindingNode parent)
    {
        this.Position = location;
        G = g;
        H = h;
        F = f;
        Parent = parent;
    }

    public bool Equals(PathfindingNode marker)
    {
        if (marker == null)
        {
            return false;
        }
        else
        {
            return this.Position.Equals(marker.Position);
        }
    }
}
