using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestsSpawner : MonoBehaviour
{
    private List<MapLocation> _possiblePlaceForChestMap = new List<MapLocation>();
    private List<MapLocation> _opositeNeighbour = new List<MapLocation>();
    [SerializeField] GameObject chestPrefab;
    [SerializeField] int chestQuantity = 0;
    [SerializeField] int chestOffset = 0;

    private void GetPossiblePlacesForSpawnChest(List<MapLocation> map)
    {
        for (int i = 0; i < map.Count; i++)
        {
            if (map[i].isclosed)
            {
                continue;
            }

            foreach (MapLocation dir in MapManager.directions)
            {
                MapLocation neighbour = new MapLocation(dir.x * MapManager.mapUnitXYScale[0], dir.z * MapManager.mapUnitXYScale[1]);
                neighbour += map[i];

                if (neighbour.x != map[i].x && neighbour.z != map[i].z)
                {
                    continue;
                }

                if (map.Exists(x => x.Equals(neighbour)))
                {
                    continue;
                }

                MapLocation opositNeighbour = new MapLocation(-dir.x * MapManager.mapUnitXYScale[0], -dir.z * MapManager.mapUnitXYScale[1]);
                opositNeighbour += map[i];

                if (map.Exists(x => x.Equals(opositNeighbour)))
                {
                    MapLocation leftNeighbour = new MapLocation(dir.z * MapManager.mapUnitXYScale[0], -dir.x * MapManager.mapUnitXYScale[1]);
                    leftNeighbour += map[i];

                    MapLocation rightNeighbour = new MapLocation(-dir.z * MapManager.mapUnitXYScale[0], dir.x * MapManager.mapUnitXYScale[1]);
                    rightNeighbour += map[i];

                    if (!map.Exists(x => x.Equals(leftNeighbour)) && !map.Exists(x => x.Equals(rightNeighbour)))
                    {
                        _possiblePlaceForChestMap.Add(map[i]);
                        _opositeNeighbour.Add(opositNeighbour);
                        break;
                    }

                    if (map.Exists(x => x.Equals(leftNeighbour)))
                    {
                        MapLocation leftCornerNeighbour = new MapLocation(0, 0);
                        
                        if(neighbour.x == map[i].x)
                        {
                            leftCornerNeighbour = new MapLocation(dir.z * MapManager.mapUnitXYScale[0], -dir.z * MapManager.mapUnitXYScale[1]);
                        }else if(neighbour.z == map[i].z)
                        {
                            leftCornerNeighbour = new MapLocation(-dir.x * MapManager.mapUnitXYScale[0], -dir.x * MapManager.mapUnitXYScale[1]);
                        }

                        leftCornerNeighbour += map[i];

                        if(!map.Exists(x => x.Equals(leftCornerNeighbour)))
                        {
                            continue;
                        }
                    }

                    if (map.Exists(x => x.Equals(rightNeighbour)))
                    {
                        MapLocation rightCornerNeighbour = new MapLocation(0, 0);

                        if (neighbour.x == map[i].x)
                        {
                            rightCornerNeighbour = new MapLocation(-dir.z * MapManager.mapUnitXYScale[0], -dir.z * MapManager.mapUnitXYScale[1]);
                        }
                        else if (neighbour.z == map[i].z)
                        {
                            rightCornerNeighbour = new MapLocation(-dir.x * MapManager.mapUnitXYScale[0], dir.x * MapManager.mapUnitXYScale[1]);
                        }

                        rightCornerNeighbour += map[i];

                        if (map.Exists(x => x.Equals(rightCornerNeighbour)))
                        {
                            _possiblePlaceForChestMap.Add(map[i]);
                            _opositeNeighbour.Add(opositNeighbour);
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

            }
        }
    }
    
    private (MapLocation, MapLocation) GetPlaceForChest()
    {
        Debug.Log("Count " + _possiblePlaceForChestMap.Count);
        int mapLocationIndex = Random.Range(0, _possiblePlaceForChestMap.Count);
        MapLocation chestPlace = new MapLocation(_possiblePlaceForChestMap[mapLocationIndex].x, _possiblePlaceForChestMap[mapLocationIndex].z);
        MapLocation oposite = new MapLocation(_opositeNeighbour[mapLocationIndex].x, _opositeNeighbour[mapLocationIndex].z);

        Debug.Log("index " + mapLocationIndex);

        int currentOffset = chestOffset * (int)MapManager.mapUnitXYScale[0];
        for (int i = 0; i < _possiblePlaceForChestMap.Count; i++)
        {

            if (_possiblePlaceForChestMap[i].x - chestPlace.x <= currentOffset || _possiblePlaceForChestMap[i].z - chestPlace.z <= currentOffset)
            {
                _possiblePlaceForChestMap.Remove(_possiblePlaceForChestMap[i]);
                _opositeNeighbour.Remove(_opositeNeighbour[i]);
            }
        }
        
        _possiblePlaceForChestMap.Remove(_possiblePlaceForChestMap.Find(x => x.Equals(chestPlace)));
        _opositeNeighbour.Remove(_opositeNeighbour.Find(x => x.Equals(oposite)));

        Debug.Log(chestPlace);
        Debug.Log(oposite);

        return (chestPlace, oposite);
    }

    private void SpawnChest()
    {
        (MapLocation, MapLocation) location = GetPlaceForChest();
        Vector3 spawnPlace = new Vector3(location.Item1.x, 0.1f, location.Item1.z);
        Vector3 lookAt = new Vector3(location.Item2.x, 0.1f, location.Item2.z);

        GameObject chest = Instantiate(chestPrefab, spawnPlace, Quaternion.identity);
        chest.transform.root.LookAt(lookAt);

        MapManager.map.Find(x => x.Equals(location.Item1)).isclosed = true;
    }


    public void SpawnChests(List<MapLocation> map)
    {
        GetPossiblePlacesForSpawnChest(map);
        chestQuantity = Mathf.Clamp(chestQuantity, 1, _possiblePlaceForChestMap.Count / chestOffset);

        for(int i = 0; i <= chestQuantity; i++)
        {
            if(_possiblePlaceForChestMap.Count > 1)
            {
                SpawnChest();
            }
            else
            {
                break;
            }
        }
    }
}
