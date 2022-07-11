using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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
        _currentCam.enabled = false;
        _virtualCam.Follow = null ;
    }
}
