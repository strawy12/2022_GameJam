using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Vector2 _offset = Vector2.zero;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void MoveBackground(float offset)
    {
        _offset.x += offset;
        _meshRenderer.material.SetTextureOffset("_BaseMap", _offset);
    }
}
