using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfTower : Tower
{
    // ���� �ڵ�
    [SerializeField] private ElfArrow _elfArrowPref;

    [SerializeField] private float _duration;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _shootPosOffset;
    [SerializeField] private float _shootForce;

    private bool isGround;

    protected override void Awake()
    {
        base.Awake();
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

    public override void UseSkill()
    {
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
        EventManager.StartListening(Constant.CLICK_SCREEN, UseSkill);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);

    }
}
