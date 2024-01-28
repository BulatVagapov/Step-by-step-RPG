using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveActionPointer : AbstractPlayerActionPointer
{
    private MeshRenderer _meshRenderer;
    [SerializeField] private Color _choosenColor;
    [SerializeField] private Color _cursorEnterColor;
    private Color _originalColor;


    public override void SelectPointer()
    {
        _isChoosen = true;
        _meshRenderer.material.color = _choosenColor;
    }

    public override void UnselectPointer()
    {
        _isChoosen = false;
        _meshRenderer.material.color = _originalColor;
    }

    public override void OnButtonClick()
    {
        StartCoroutine(MapManager.player.Movement.MoveAction(MapManager.map.Find(x => x.Equals(MapManager.playerAction.transform.position))));
    }

    protected override void OnMouseOver()
    {
        if (!_isChoosen && !EventSystem.current.IsPointerOverGameObject())
            _meshRenderer.material.color = _cursorEnterColor;
    }

    protected override void OnMouseExit()
    {
        if (!_isChoosen && !EventSystem.current.IsPointerOverGameObject())
            _meshRenderer.material.color = _originalColor;
    }

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
    }
}
