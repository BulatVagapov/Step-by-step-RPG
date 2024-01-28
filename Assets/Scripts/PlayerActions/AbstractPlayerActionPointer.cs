using UnityEngine;
using UnityEngine.EventSystems;

public abstract class AbstractPlayerActionPointer : MonoBehaviour
{
    protected bool _isChoosen;
    [SerializeField] protected string buttonText;
   
    public abstract void SelectPointer();
    public abstract void UnselectPointer();

    public abstract void OnButtonClick();

    public virtual void RemoveMethod()
    {
        MapManager.actionBatton.onClick?.RemoveAllListeners();
        MapManager.playerAction = null;
    }

    protected abstract void OnMouseOver();
    protected abstract void OnMouseExit();
    protected virtual void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (!_isChoosen)
            {
                MapManager.playerAction?.UnselectPointer();

                MapManager.playerAction?.RemoveMethod();

                MapManager.actionBatton.onClick.AddListener(this.OnButtonClick);

                SelectPointer();
                MapManager.playerAction = this;
                MapManager.actionButtonText.text = buttonText;
            }
            else
            {
                UnselectPointer();
                MapManager.playerAction?.RemoveMethod();
                MapManager.actionButtonText.text = "Skip a turn";
            }
        }
        
       
    }
}
