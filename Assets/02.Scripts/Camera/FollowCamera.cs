using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private float _followDelay;
    [SerializeField] private float _endCameraStayTime;
    private Camera _currentCam;

    private void Awake()
    {
        _currentCam = GetComponent<Camera>();
    }

    public void SetTarget(Transform target)
    {
        _virtualCam.Follow = target;
    }

    public void StartFollow()
    {
        StartCoroutine(FollowDelay());
    }

    private IEnumerator FollowDelay()
    {
        yield return new WaitForSeconds(_followDelay);
        _virtualCam.enabled = true;
        _currentCam.enabled = true;
    }


    public void EndFollow()
    {
        StopAllCoroutines();

        Camera mainCam = Define.MainCam;
        mainCam.transform.position = _currentCam.transform.position + Vector3.down;

        _currentCam.enabled = false;
        _virtualCam.Follow = null ;
        _virtualCam.enabled = false;

        Sequence seq = DOTween.Sequence();

        seq.Append(mainCam.DOShakePosition(0.5f, 1.5f, 10));
        seq.AppendInterval(_endCameraStayTime);
        seq.Append(GameManager.Inst.MainCameraMove.MoveCameraPos(new Vector3(13f, 0f, -10f), 0.5f));
        seq.AppendCallback(() => 
        {
            GameManager.Inst.gameState = GameManager.GameState.Game;
        });
        
    }


}
