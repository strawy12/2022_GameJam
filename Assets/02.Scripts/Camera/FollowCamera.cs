using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    private Camera _currentCam;

    private void Awake()
    {
        _currentCam = GetComponent<Camera>();
    }

    private void Start()
    {
        EventManager<Transform>.StartListening(Constant.START_THROW_TOWER, StartFollow);
        EventManager.StartListening(Constant.END_THROW_TOWER, EndFollow);
    }

    public void StartFollow(Transform target)
    {
        _virtualCam.Follow = target;
        _currentCam.enabled = true;
    }

    public void EndFollow()
    {
        Camera mainCam = Define.MainCam;
        Vector3 originPos = mainCam.transform.position ;
        Debug.Log(originPos);

        mainCam.transform.position = _currentCam.transform.position + Vector3.down;

        _currentCam.enabled = false;
        _virtualCam.Follow = null ;
        
        Sequence seq = DOTween.Sequence();
        seq.Append(mainCam.DOShakePosition(0.5f, 1.5f, 10));
        seq.Append(mainCam.transform.DOMove(originPos, 0.75f));
    }

    private void OnDestroy()
    {
        EventManager<Transform>.StopListening(Constant.START_THROW_TOWER, StartFollow);
        EventManager.StopListening(Constant.END_THROW_TOWER, EndFollow);
    }

    private void OnApplicationQuit()
    {
        EventManager<Transform>.StopListening(Constant.START_THROW_TOWER, StartFollow);
        EventManager.StopListening(Constant.END_THROW_TOWER, EndFollow);
    }
}
