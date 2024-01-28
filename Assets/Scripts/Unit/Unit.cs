using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UnitMovement), typeof(UnitHP), typeof(UnitDamageDealer))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Animator Animator { get { return _animator; } }
    [SerializeField] private GameStates _state;
    public GameStates State { get { return _state; } }

    [SerializeField] private string _enemy;

    private UnitMovement _movement;
    public UnitMovement Movement { get { return _movement; } }
    private UnitHP _hp;
    public UnitHP HP { get { return _hp; } }
    private UnitDamageDealer _damageDealer;
    public UnitDamageDealer DamageDealer { get { return _damageDealer; } }

    private UnityEvent turnIsOverEvent = new UnityEvent();
    public UnityEvent TurnIsOverEvent { get { return turnIsOverEvent; } set { turnIsOverEvent = value; } }

    public void AttackAction(UnitHP hp)
    {
        StartCoroutine(_damageDealer.DealDamage(hp));
    }

    private void Awake()
    {
        _movement = GetComponent<UnitMovement>();
        _hp = GetComponent<UnitHP>();
        _damageDealer = GetComponent<UnitDamageDealer>();
    }
}
