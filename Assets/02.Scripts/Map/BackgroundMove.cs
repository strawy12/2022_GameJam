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

    }


    public void Update() 
    {
        if (_isThrowing == false) return;
        if (_meshRenderer == null) return;

        _offset.x += _moveSpeed * Time.deltaTime;
        _meshRenderer.material.SetTextureOffset("_BaseMap", _offset);
    }

}
