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
        Instantiate(_swampObj); // ���ӸŴ��� Ǯ�� �Ƕ�̵�Swamp�˻���
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
