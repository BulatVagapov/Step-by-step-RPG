using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private int _randomBorder;
    private List<int> _possibleMaplocationIndeces = new List<int>();
    private List<int> _removedIndeces = new List<int>();

    public void PossibleMapLocationIndecesInitialization(List<MapLocation> map)
    {
        for(int i = 0; i < map.Count; i++)
        {
            if (!map[i].isclosed)
            {
                _possibleMaplocationIndeces.Add(i);
            }
        }

        _possibleMaplocationIndeces.Sort();
    }

    private void PreparePossibleMapLocationIndecesForSearch(int enemyMapLocationIndex)
    {
        int enemylocaton = _possibleMaplocationIndeces[enemyMapLocationIndex];

        int lowBorder = enemylocaton - _randomBorder < 0 ? 0 : enemylocaton - _randomBorder;
        int topBorder = enemyMapLocationIndex + _randomBorder >= _possibleMaplocationIndeces.Count ? _possibleMaplocationIndeces.Count - 1 : enemyMapLocationIndex + _randomBorder;

        for(int i = 0; i < _possibleMaplocationIndeces.Count; i++)
        {
            if(_possibleMaplocationIndeces[i] >= lowBorder && _possibleMaplocationIndeces[i] <= topBorder)
            {
                _removedIndeces.Add(_possibleMaplocationIndeces[i]);
                _possibleMaplocationIndeces.RemoveAt(i);
            }
        }
    }

    public int FindPatrolTargetLocation(int enemyMapLocationIndex)
    {
        PreparePossibleMapLocationIndecesForSearch(enemyMapLocationIndex);

        _possibleMaplocationIndeces.Sort();

        int index = Random.Range(0, _possibleMaplocationIndeces.Count);

        return index;
    }

    public List<UnitAI> SpawnEnemy(int enemyQuantity)
    {
        List<UnitAI> enemies = new List<UnitAI>();
        
        for(int i = 0; i < enemyQuantity; i++)
        {
            int mapLocationIndex = Random.Range(0, _possibleMaplocationIndeces.Count);
            MapManager.map[_possibleMaplocationIndeces[mapLocationIndex]].isclosed = true;

            Vector3 enemyPosition = new Vector3(MapManager.map[_possibleMaplocationIndeces[mapLocationIndex]].x, 0.1f, MapManager.map[_possibleMaplocationIndeces[mapLocationIndex]].z);

            GameObject enemy = Instantiate(_enemyPrefab, enemyPosition, Quaternion.identity);

            enemy.name = $"Enemy {i}";
            enemies.Add(enemy.GetComponent<UnitAI>());

            enemies[i].PatrolTargetPosition = MapManager.map[_possibleMaplocationIndeces[FindPatrolTargetLocation(mapLocationIndex)]];
            enemies[i].transform.LookAt(new Vector3(enemies[i].PatrolTargetPosition.x, enemies[i].transform.position.y, enemies[i].PatrolTargetPosition.z));

            for(int f = 0; f < _removedIndeces.Count; f++)
            {
                _possibleMaplocationIndeces.Add(_removedIndeces[i]);
            }
        }

        return enemies;
    }
}
