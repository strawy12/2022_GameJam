using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidSwamp : PoolableMono
{
    [SerializeField] private float _swampPosX;
    [SerializeField] private float _swampPosY;

    private void OnEnable()
    {
        StartCoroutine(MakePyramidSwamp());
    }

    public void Init(float posX, float posY)
    {
        _swampPosX = posX;
        _swampPosY = posY;
    }

    IEnumerator MakePyramidSwamp()
    {
        transform.position = new Vector2(_swampPosX, _swampPosY);
        yield return new WaitForSeconds(3f);
        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }
}
