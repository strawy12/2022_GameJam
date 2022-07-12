using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileControl : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerClickHandler
{

    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.Inst.gameState == GameManager.GameState.Game)
        {
            GameManager.Inst.MainCameraMove.CameraMove(-eventData.delta.x);
        }


    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Inst.gameState == GameManager.GameState.Throwing)
        {
            EventManager.TriggerEvent(Constant.CLICK_SCREEN);
        }
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (GameManager.Inst.gameState == GameManager.GameState.Game)
        {
            GameManager.Inst.MainCameraMove.CameraMove(0);
        }
    }
}
