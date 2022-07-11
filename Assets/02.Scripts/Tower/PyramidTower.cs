using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidTower : Tower
{
    [SerializeField] private EdgeCollider2D _attackCol;

    protected override void Awake()
    {
        base.Awake();
        _attackCol.enabled = false;
    }

    private void Update()
    {
        if(_attackCol.CompareTag("Enemy"))
        {
            // 몬스터한테 데미지주기, 속도 느리게하기
        }
    }

    IEnumerator ExPlosionEffect()
    {
        yield return new WaitForSeconds(1f);
        _attackCol.enabled = true;
        DestroyTower();
    }

    public override void UseSkill()
    {
        StartCoroutine(ExPlosionEffect());
    }
}
