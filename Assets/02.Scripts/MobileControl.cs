using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileControl : MonoBehaviour, /*IDragHandler, IEndDragHandler,*/ IPointerDownHandler
{

    //public void OnDrag(PointerEventData eventData)
    //{
    //    switch (GameManager.Inst.gameState)
    //    {
    //        case GameManager.GameState.Game:
    //        case GameManager.GameState.UI:
    //            GameManager.Inst.MainCameraMove.CameraMove(-eventData.delta.x);
    //            break;
    //    }
    //}
    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Inst.gameState == GameManager.GameState.Game)
        {
            EventManager.TriggerEvent(Constant.CLICK_SCREEN);
            //case GameManager.GameState.Game:
            //case GameManager.GameState.UI:
            //    GameManager.Inst.MainCameraMove.StopImmediately();
            //    break;
        }
    }

    //void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    //{
    //    switch (GameManager.Inst.gameState)
    //    {
    //        case GameManager.GameState.Game:
    //        case GameManager.GameState.UI:
    //            GameManager.Inst.MainCameraMove.CameraMove(0);

    //            break;
    //    }
    //}
}
