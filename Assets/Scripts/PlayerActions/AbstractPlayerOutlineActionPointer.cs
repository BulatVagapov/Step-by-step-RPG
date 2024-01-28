using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class AbstractPlayerOutlineActionPointer : AbstractPlayerActionPointer
{
    protected Outline _outline;
    
    [SerializeField] protected Color _choosenColor;
    protected Color _originalColor;
    [SerializeField] protected Color _cursorEnterColor;

    protected virtual void OnEnable()
    {
        _outline.enabled = true;
    }

    protected virtual void OnDisable()
    {
        _outline.enabled = false;
    }

    public override void OnButtonClick()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void Awake()
    {
        _outline = GetComponent<Outline>();
        _originalColor = _outline.OutlineColor;
        PlayerOutlineActionPointersManager.playerActions.Add(this);
    }

    protected override void OnMouseOver()
    {
        if (!_isChoosen)
            _outline.OutlineColor = _cursorEnterColor;
    }

    protected override void OnMouseExit()
    {
        if (!_isChoosen)
            _outline.OutlineColor = _originalColor;
    }

    public override void SelectPointer()
    {
        _isChoosen = true;
        _outline.OutlineColor = _choosenColor;
    }

    public override void UnselectPointer()
    {
        _isChoosen = false;
        _outline.OutlineColor = _originalColor;
    }

    public virtual void RemoveFromPointers()
    {
        PlayerOutlineActionPointersManager.playerActions.Remove(this);
    }
}
