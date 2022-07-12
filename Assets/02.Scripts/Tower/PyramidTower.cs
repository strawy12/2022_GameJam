using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidTower : Tower
{
    [SerializeField] private PyramidSwamp _swampObj;

    protected override void Awake()
    {
        base.Awake();
    }

    IEnumerator ExPlosionEffect()
    {
        _swampObj.Init(transform.position.x, transform.position.y - .9f);
        Instantiate(_swampObj); // 게임매니저 풀로 피라미드Swamp늪생성
        yield return new WaitForSeconds(1f);
        DestroyTower();
    }

    public override void UseSkill()
    {
        StartCoroutine(ExPlosionEffect());
    }

    private void OnMouseDown()
    {
        UseSkill();
    }
}
