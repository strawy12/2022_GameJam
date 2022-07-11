using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfTower : Tower
{
    // 버릴 코드
    [SerializeField] private ElfArrow _elfArrowPref;

    [SerializeField] private float _duration;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _shootPosOffset;
    [SerializeField] private float _shootForce;
    protected override void Awake()
    {
        base.Awake();
        new PoolManager(transform);
        PoolManager.Instance.CreatePool(_elfArrowPref);
    }

    IEnumerator ElfTowerSkill()
    {
        float time = _duration;
        Vector2 shootPos = transform.position;
        while (time > 0)
        {
            ElfArrow arrow = PoolManager.Instance.Pop("ElfArrow") as ElfArrow;
            arrow.transform.position = shootPos + new Vector2(_shootPosOffset* Random.Range(-1, 1), 0f);
            arrow.gameObject.SetActive(true);
            arrow.Init(Vector2.down, _shootForce);
            yield return new WaitForSeconds(_shootDelay);
            time -= _shootDelay;
        }
    }

    [ContextMenu("스킬사용")]
    public override void UseSkill()
    {
        StartCoroutine(ElfTowerSkill());
    }

}
