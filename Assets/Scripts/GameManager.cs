using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Events;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _waitingPlayerTurnTime;
    [SerializeField] private Unit _player;
    [SerializeField] private Button _button;
    [SerializeField] private int _enemyQuanity;
    private EnemySpawner _enemySpawner;
    private ChestsSpawner _chestSpawner;
    private PlayerMoveActionPointersManager _playerMoveIndicatorsManager;
    private List<UnitAI> _enemies = new List<UnitAI>();
    private int _enemyIndex;
    [SerializeField] private Transform _tilemapTransform;

    public IEnumerator WaitingPlayerTurn()
    {
        yield return new WaitForSeconds(_waitingPlayerTurnTime);
        GameStateManager.state = GameStates.Lose;
        Debug.Log("you returned to hall");
    }

    void Start()
    {
        _player.TurnIsOverEvent.AddListener(EnemyTurn);


        foreach (UnitAI enemy in _enemies)
        {
            enemy._Unit.TurnIsOverEvent.AddListener(SwitchToWaitingPlayerTurnWhenAllEnemiesMadeTurn);
        }
        PlayerOutlineActionPointersManager.HideplayerActionPointers();
        WaitingPlayerActionTurn();
    }

    public void OnButtonClick()
    {
        GameStateManager.state = GameStates.PlayerTurn;
        StopCoroutine("WaitingPlayerTurn");
        MapManager.playerAction?.UnselectPointer();
        _playerMoveIndicatorsManager.HideIndicators();
        PlayerOutlineActionPointersManager.HideplayerActionPointers();
        MapManager.actionBatton.interactable = false;

        if (MapManager.playerAction == null)
        {
            EnemyTurn();
        }
    }

    private void EnemyTurn()
    {
        MapManager.playerAction?.RemoveMethod();

        GameStateManager.state = GameStates.EnemysTurn;
        
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i].Unit.HP.IsDead)
            {
                _enemies.Remove(_enemies[i]);
            }
        }

        if(_enemies.Count <= 0)
        {
            WaitingPlayerActionTurn();
            return;
        }

        foreach (UnitAI unit in _enemies)
        {
            unit.SelectAction();
        }
    }

    private void WaitingPlayerActionTurn()
    {
        GameStateManager.state = GameStates.WaitingPlayerTurn;
        StartCoroutine("WaitingPlayerTurn");
        _playerMoveIndicatorsManager.ShowIndicators();
        PlayerOutlineActionPointersManager.ShowPlayerActionPointers();
        MapManager.actionButtonText.text = "Skip a turn";
        MapManager.actionBatton.interactable = true;
    }

    private void SwitchToWaitingPlayerTurnWhenAllEnemiesMadeTurn()
    {
        _enemyIndex++;

        if (_enemyIndex >= _enemies.Count && GameStateManager.state.Equals(GameStates.EnemysTurn))
        {
            WaitingPlayerActionTurn();
            _enemyIndex = 0;
        }
    }

    private void Awake()
    {
        _enemySpawner = GetComponent<EnemySpawner>();
        _chestSpawner = GetComponent<ChestsSpawner>();

        MapManager.SetMapFromTilemapTransform(_tilemapTransform);
        _playerMoveIndicatorsManager = _player.GetComponent<PlayerMoveActionPointersManager>();
        _playerMoveIndicatorsManager.IndicatorsInitialization();
        _enemyIndex = 0;
        MapManager.map.Find(x => x.Equals(_player.transform.position)).isclosed = true;

        _chestSpawner.SpawnChests(MapManager.map);

        _enemySpawner.PossibleMapLocationIndecesInitialization(MapManager.map);

        _enemies = _enemySpawner.SpawnEnemy(_enemyQuanity);

        MapManager.player = _player;
        MapManager.actionBatton = _button;
        MapManager.actionButtonText = MapManager.actionBatton.transform.GetChild(1).GetComponent<Text>();
        UnityEventTools.AddPersistentListener(MapManager.actionBatton.onClick, this.OnButtonClick);
    }
}
