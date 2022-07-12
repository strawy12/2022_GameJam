using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 0.1f;
    private MeshRenderer _meshRenderer;
    private Vector2 _offset = Vector2.zero;

    private bool _isThrowing;
    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();

        EventManager.StartListening(Constant.START_THROW_TOWER, () => _isThrowing = true);
        EventManager.StartListening(Constant.END_THROW_TOWER, () => _isThrowing = false);
    }


    public void Update() 
    {
        if (_isThrowing == false) return;
        if (_meshRenderer == null) return;

        _offset.x += _moveSpeed * Time.deltaTime;
        _meshRenderer.material.SetTextureOffset("_BaseMap", _offset);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Constant.START_THROW_TOWER, () => _isThrowing = true);
        EventManager.StopListening(Constant.END_THROW_TOWER, () => _isThrowing = false);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(Constant.START_THROW_TOWER, () => _isThrowing = true);
        EventManager.StopListening(Constant.END_THROW_TOWER, () => _isThrowing = false);
    }
}
