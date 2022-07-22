using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SubCamera : MonoBehaviour
{
    [SerializeField] private CanvasGroup _subCameraUI;
     
    public void ShowSubCamUI(Vector3 targetPos)
    {
        _subCameraUI.DOKill();
        transform.position = targetPos;
        _subCameraUI.alpha = 1f;

        _subCameraUI.DOFade(0f, 0.5f).SetDelay(3f);
    }


}
