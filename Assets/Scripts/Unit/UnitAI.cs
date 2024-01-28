using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AStarPathfinding), typeof(Unit))]
public class UnitAI : MonoBehaviour
{
    [SerializeField] private float _overlapDistance;
    [SerializeField] private LayerMask _playerLM;
    private MapLocation _patrolTargetPosition;
    public MapLocation PatrolTargetPosition { get { return _patrolTargetPosition; } set { _patrolTargetPosition = value; } }
    private MapLocation _startPatrolPosition;
    private MapLocation _currentGoalLocation;
    private Unit _unit;
    private List<MapLocation> _path = new List<MapLocation>();

    public Unit _Unit { get { return _unit; } }
    private AStarPathfinding _pathfinder;
    private int _pathIndex;
    private int _indexCoef;
    private bool _onPatrolRoute;

    public GameObject pathPObjPrefab;
    public Material pathPMaterial;

    public Unit Unit { get { return _unit; } }

    public void SelectAction()
    {
        Collider[] collisionsInDistance = Physics.OverlapSphere(transform.position, _overlapDistance * MapManager.mapUnitXYScale[0], _playerLM);

        if(collisionsInDistance.Length > 0)
        {
            if(Mathf.Abs(Vector3.Distance(transform.position, collisionsInDistance[0].transform.position)) < 1.5f * MapManager.mapUnitXYScale[0])
            {
                if(!MapManager.IsHasNeighbours(collisionsInDistance[0].transform.position, transform.position))
                {
                    GetCurrentPath(transform.position, collisionsInDistance[0].transform.position);
                    _pathIndex = Mathf.Clamp(_path.Count - 2, 0, _path.Count -1);
                    _onPatrolRoute = false;
                    StartCoroutine(_unit.Movement.MoveAction(_path[_pathIndex]));
                }
                else
                {
                    _unit.AttackAction(collisionsInDistance[0].GetComponent<UnitHP>());
                }
            }
            else
            {
                GetCurrentPath(transform.position, collisionsInDistance[0].transform.position);
                _pathIndex = Mathf.Clamp(_path.Count - 2, 0, _path.Count - 1);
                _onPatrolRoute = false;
                StartCoroutine(_unit.Movement.MoveAction(_path[_pathIndex]));
            }

        }
        else
        {
            if (!_onPatrolRoute)
            {
                GetCurrentPath(transform.position, _currentGoalLocation.ToVector());
                _pathIndex = Mathf.Clamp(_path.Count - 2, 0, _path.Count - 1);
                _onPatrolRoute = true;
            }
            else
            {
                _pathIndex += _indexCoef;

                if ((_pathIndex < 0 && !_path[0].Equals(_currentGoalLocation)) ||
                    (_pathIndex >= 0 && _pathIndex < _path.Count && _path[_pathIndex].willBeClosed) ||
                    (_pathIndex >= 0 && _pathIndex < _path.Count && MapManager.WillEnemyMeet(_path[_pathIndex].ToVector(), transform.position)) ||
                    (((_pathIndex < 0 && _path[0].Equals(_currentGoalLocation)) || _pathIndex == _path.Count) && !IsPatrolPathCorrect()))
                {
                    GetPossiblePath(transform.position, _currentGoalLocation.ToVector());
                    _pathIndex = Mathf.Clamp(_path.Count - 2, 0, _path.Count - 1);
                }

                if (((_pathIndex < 0 && _path[0].Equals(_currentGoalLocation)) || _pathIndex == _path.Count) && IsPatrolPathCorrect())
                {
                    _pathIndex = _pathIndex < 0 ? Mathf.Clamp(1, 0, _path.Count - 1) : Mathf.Clamp(_path.Count - 2, 0, _path.Count - 1);

                    _indexCoef *= -1;

                    CurrentPathTargetInverter();
                }

                StartCoroutine(_unit.Movement.MoveAction(_path[_pathIndex]));
            }
        }
    }

    private void GetCurrentPath(Vector3 startPosition, Vector3 goalPosition)
    {
        _pathfinder.BeginSearch(startPosition, goalPosition);
        _pathfinder.SearchAllNodes();
        _path = _pathfinder.GetPath();
        _indexCoef = -1;
    }

    private void GetPossiblePath(Vector3 startPosition, Vector3 goalPosition)
    {
        GetCurrentPath(startPosition, goalPosition);

        if (_path.Count > 1)
        {
            return;
        }
        else
        {
            CurrentPathTargetInverter();

            GetCurrentPath(startPosition, _currentGoalLocation.ToVector());
        }
    }

    private bool IsPatrolPathCorrect()
    {
        bool pathCorrect = false;
        
        if(_pathIndex < 0)
        {
            if(_currentGoalLocation == _startPatrolPosition && _path[_path.Count - 1].Equals(_patrolTargetPosition))
            {
                pathCorrect = true;
            }else if(_currentGoalLocation == _patrolTargetPosition && _path[_path.Count - 1].Equals(_startPatrolPosition))
            {
                pathCorrect = true;
            }

        }else if(_pathIndex == _path.Count)
        {
            if (_currentGoalLocation == _startPatrolPosition && _path[0].Equals(_patrolTargetPosition))
            {
                pathCorrect = true;
            }else if (_currentGoalLocation == _patrolTargetPosition && _path[0].Equals(_startPatrolPosition))
            {
                pathCorrect = true;
            }
        }

        return pathCorrect;
    }

    private void CurrentPathTargetInverter()
    {
        _currentGoalLocation = _currentGoalLocation == _patrolTargetPosition ? _startPatrolPosition : _patrolTargetPosition;
    }
    
    void Awake()
    {
        _unit = GetComponent<Unit>();
        _pathfinder = GetComponent<AStarPathfinding>();
    }

    private void Start()
    {
        _startPatrolPosition = MapManager.map.Find(x => x.Equals(transform.position));
        GetCurrentPath(_startPatrolPosition.ToVector(), _patrolTargetPosition.ToVector());
        _currentGoalLocation = _patrolTargetPosition;
        _onPatrolRoute = true;

        _pathIndex = _path.Count > 1 ? _path.Count - 1 : 1;
        MapManager.map.Find(x => x.Equals(transform.position)).isclosed = true;
    }
}
