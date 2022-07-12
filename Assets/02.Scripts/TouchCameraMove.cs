using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCameraMove : MonoBehaviour, IDragHandler, IEndDragHandler
{

    public void OnDrag(PointerEventData eventData)
    {
        GameManager.Inst.MainCameraMove.CameraMove(-eventData.delta.x);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        GameManager.Inst.MainCameraMove.CameraMove(0);
    }
}

