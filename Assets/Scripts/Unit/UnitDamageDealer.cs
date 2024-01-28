using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDamageDealer : MonoBehaviour
{
    [SerializeField] private float _damage;
    private Unit _unit;
    
    public IEnumerator DealDamage(UnitHP hp)
    {
        _unit.Animator.SetBool("walk", true);

        while (!_unit.Movement.RotateUnitToTarget(MapManager.map.Find(x => x.Equals(hp.transform.position))))
        {
            yield return new WaitForEndOfFrame();
        }

        _unit.Animator.SetBool("walk", false);
        _unit.Animator.SetBool("attack", true);

        while (!_unit.Animator.GetCurrentAnimatorStateInfo(0).IsName("Male Attack 1"))
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(_unit.Animator.GetCurrentAnimatorStateInfo(0).length / 10);
        hp.TakeDamage(_damage);
        yield return new WaitForSeconds(_unit.Animator.GetCurrentAnimatorStateInfo(0).length);
        _unit.Animator.SetBool("attack", false);
        _unit.TurnIsOverEvent?.Invoke();
    }

    private void Awake()
    {
        _unit = GetComponent<Unit>();
    }
}
