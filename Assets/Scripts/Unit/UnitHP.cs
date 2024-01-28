using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitHP : MonoBehaviour
{
    [SerializeField] private float _hp;
    private Unit _unit;

    private bool _isDead = false;
    public bool IsDead { get { return _isDead; } }

    public UnityEvent unitIsDeadEvent = new UnityEvent();

    private IEnumerator TakeDamageAction(float damage)
    {
        _unit.Animator.SetBool("tookDamage", true);
        _hp -= damage;
        if (_hp <= 0)
        {
            _unit.Animator.SetBool("dead", true);
            unitIsDeadEvent?.Invoke();
            _unit.Movement.MapPosition.isclosed = false;
            _isDead = true;
        }

        yield return new WaitForSeconds(0.8f);
        _unit.Animator.SetBool("tookDamage", false);
    }

    public void TakeDamage(float damage)
    {
        if (!_isDead)
        {

            StartCoroutine(TakeDamageAction(damage));
        }
    }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }
}
