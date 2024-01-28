using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AStarPathfinding : MonoBehaviour
{
    private List<PathfindingNode> _open = new List<PathfindingNode>();
    public List<PathfindingNode> Open { get { return _open; } }
    private List<PathfindingNode> _closed = new List<PathfindingNode>();
    public List<PathfindingNode> Closed { get { return _closed; } }

    private PathfindingNode _startNode;
    public PathfindingNode StartNode { get { return _startNode; } }
    private PathfindingNode _endNode;
    public PathfindingNode EndNode { get { return _endNode; } }

    private PathfindingNode _lastPosition;
    public PathfindingNode LastPosition { get { return _lastPosition; } }

    private bool done = false;

    public void BeginSearch(Vector3 startPosition, Vector3 goalPosition)
    {
        done = false;
        
        _endNode = new PathfindingNode(MapManager.map.Find(x => x.Equals(goalPosition)), 0, 0, 0, null);

        _startNode = new PathfindingNode(MapManager.map.Find(x => x.Equals(startPosition)), 0, Vector3.Distance(startPosition, goalPosition), 0, null);

        _open.Clear();
        _closed.Clear();
        _open.Add(_startNode);
        _lastPosition = _startNode;
    }

    public void Search(PathfindingNode thisNode)
    {
        if (thisNode == null) return;
        if (thisNode.Equals(_endNode))
        {
            done = true;
            _closed.Add(thisNode);
            return;//goal has been found
        }
        
        foreach (MapLocation dir in MapManager.directions)
        {
            MapLocation neighbour = new MapLocation(dir.x * MapManager.mapUnitXYScale[0], dir.z * MapManager.mapUnitXYScale[1]);
            neighbour += thisNode.Position;

            if (!MapManager.map.Exists(x => x.Equals(neighbour)))
            {
                continue;
            }

            if (MapManager.map.Find(x => x.Equals(neighbour)).willBeClosed)
            {
                if (neighbour.Equals(_endNode.Position) && MapManager.IsHasNeighbours(neighbour.ToVector(), thisNode.Position.ToVector()))
                {
                    _endNode = thisNode;
                    done = true;
                    return;
                }
                else
                {
                    continue;
                }
            }

            if (!MapManager.IsHasNeighbours(neighbour.ToVector(), thisNode.Position.ToVector()))
            {
                continue;
            }

            if(MapManager.WillEnemyMeet(neighbour.ToVector(), thisNode.Position.ToVector()))
            {
                continue;
            }

            if (IsClosed(neighbour))
            {
                continue;
            }

            float g = Vector3.Distance(thisNode.Position.ToVector(), neighbour.ToVector()) + thisNode.G;
            float h = Vector3.Distance(neighbour.ToVector(), _endNode.Position.ToVector());
            float f = g + h;

            if(!UpdateMarker(MapManager.map.Find(x => x.Equals(neighbour)), g, h, f, thisNode))
            {
                _open.Add(new PathfindingNode(MapManager.map.Find(x => x.Equals(neighbour)), g, h, f, thisNode));
            }
        }

        if(_open.Count == 0)
        {
            _closed = _closed.OrderBy(x => x.H).ToList<PathfindingNode>();
            PathfindingNode nearestNode = _closed.ElementAt(0);

            _lastPosition = nearestNode;

            _endNode = _lastPosition;
            done = true;
            return;
        }
        
        _open = _open.OrderBy(p => p.F).ToList<PathfindingNode>();
        PathfindingNode pn = (PathfindingNode)_open.ElementAt(0);
        _closed.Add(pn);

        _open.RemoveAt(0);

        _lastPosition = pn;
    }

    private bool UpdateMarker(MapLocation pos, float g, float h, float f, PathfindingNode node)
    {
        foreach(PathfindingNode p in _open)
        {
            if (p.Position.Equals(pos))
            {
                p.G = g;
                p.H = h;
                p.F = f;
                p.Parent = node;
                return true;
            }
        }

        return false;
    }

    private bool IsClosed(MapLocation marker)
    {
        foreach(PathfindingNode p in _closed)
        {
            if (p.Position.Equals(marker))
            {
                return true;
            }
        }

        return false;
    }

    public void SearchAllNodes()
    {
        while (!done)
        {
            Search(LastPosition);
        }
    }

    public List<MapLocation> GetPath()
    {
        List<MapLocation> path = new List<MapLocation>();
        PathfindingNode begin = _lastPosition;
        while (!_startNode.Equals(begin) && begin != null)
        {
            path.Add(begin.Position);
            begin = begin.Parent;
        }

        path.Add(_startNode.Position);
        return path;
    }
}
