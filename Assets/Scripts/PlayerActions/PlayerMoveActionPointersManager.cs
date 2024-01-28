using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveActionPointersManager : MonoBehaviour
{
    [SerializeField] private GameObject _indicatorPrefab;
    private List<(MeshRenderer, MeshCollider)> _indicators;
    
    public void IndicatorsInitialization()
    {
        _indicators = new List<(MeshRenderer, MeshCollider)>();

        foreach (MapLocation dir in MapManager.directions)
        {
            MapLocation neighbour = new MapLocation(dir.x * MapManager.mapUnitXYScale[0], dir.z * MapManager.mapUnitXYScale[1]);
            neighbour = new MapLocation(neighbour.x + transform.position.x, neighbour.z + transform.position.z);

            GameObject indicator = Instantiate(_indicatorPrefab, new Vector3(neighbour.x, transform.position.y, neighbour.z), Quaternion.Euler(90, 0, 0));
            Vector3 indicatorScale = new Vector3(indicator.transform.localScale.x * MapManager.mapUnitXYScale[0], indicator.transform.localScale.y * MapManager.mapUnitXYScale[1], indicator.transform.localScale.z);
            indicator.transform.localScale = indicatorScale;
            indicator.transform.parent = transform;
            (MeshRenderer, MeshCollider) pointer = (indicator.GetComponent<MeshRenderer>(), indicator.GetComponent<MeshCollider>());
            _indicators.Add(pointer);
        }

        HideIndicators();
    }

    public void ShowIndicators()
    {
        foreach((MeshRenderer, MeshCollider) p in _indicators)
        {
            if (!MapManager.map.Exists(x => x.Equals(p.Item1.transform.position)))
            {
                continue;
            }

            if (MapManager.map.Find(x => x.Equals(p.Item1.transform.position)).isclosed)
            {
                continue;
            }

            if (!MapManager.IsHasNeighbours(p.Item1.transform.position, transform.position))
            {
                continue;
            }

            p.Item1.enabled = true;
            p.Item2.enabled = true;
        }
    }

    public void HideIndicators()
    {
        foreach ((MeshRenderer, MeshCollider) p in _indicators)
        {
            if (p.Item1.enabled)
                p.Item1.enabled = false;

            if (p.Item2.enabled)
                p.Item2.enabled = false;
        }
    }
}
