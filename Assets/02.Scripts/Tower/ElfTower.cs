using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfTower : Tower
{
    [SerializeField] private ElfArrow _elfArrowPref;

    [SerializeField] private float _duration;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _shootPosOffset;
    [SerializeField] private float _shootForce;

    IEnumerator ElfTowerSkill()
    {
        Vector2 shootPos = transform.position;

        for (int i = 0; i < 8; i++)
        {
            ElfArrow arrow = PoolManager.Instance.Pop("ElfArrow") as ElfArrow;
            arrow.transform.position = shootPos + new Vector2(_shootPosOffset * Random.Range(-1f, 1f), 0f);
            arrow.Init(Vector2.down, _shootForce);
            yield return new WaitForSeconds(_shootDelay);
        }
    }
    protected override void OnThrowTower()
    {
        EventManager.StartListening(Constant.CLICK_SCREEN, UseSkill);
    }
    public override void UseSkill()
    {
        Debug.Log("useElf");
        if (gameObject.activeSelf == false) return;

        StartCoroutine(ElfTowerSkill());
    }


    public override void DestroyTower()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);
        base.DestroyTower();
    }

    public override void Reset()
    {
        base.Reset();
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);
    }

    protected override void SpawnEffect()
    {
        throw new System.NotImplementedException();
    }
}
