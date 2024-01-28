using UnityEngine;

[RequireComponent(typeof(Outline))]
public class PlayerAttackActionPointer : AbstractPlayerOutlineActionPointer
{
    private Unit _unit;

    public override void OnButtonClick()
    {
        StartCoroutine(MapManager.player.DamageDealer.DealDamage(_unit.HP));
    }

    protected override void Awake()
    {
        base.Awake();
        _unit = GetComponent<Unit>();
    }

    private void Start()
    {
        _unit.HP.unitIsDeadEvent.AddListener(RemoveFromPointers);
    }
}
