using UnityEngine;

public class PlayerOpenChestActionPointer : AbstractPlayerOutlineActionPointer
{
    private Chest _chest;

    public override void OnButtonClick()
    {
        StartCoroutine(_chest.OpenChest());
    }

    protected override void Awake()
    {
        base.Awake();
        _chest = GetComponent<Chest>();
        _chest.ChestIsOpenEvent.AddListener(RemoveFromPointers);
    }
}
