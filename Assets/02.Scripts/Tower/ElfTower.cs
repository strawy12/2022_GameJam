using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElfTower : Tower
{
    private bool isGround;


    [SerializeField] private ElfArrow _elfArrowPref;

    [SerializeField] private float _duration;
    [SerializeField] private float _shootDelay;
    [SerializeField] private float _shootPosOffset;
    [SerializeField] private float _shootForce;

    public UnityEvent OnSkillShot;
    public bool isUsed = false;
    protected override void Awake()
    {
        base.Awake();
    }

    IEnumerator ElfTowerSkill()
    {
        isUsed = true;
        Vector2 shootPos = transform.position;

        for (int i = 0; i < 8; i++)
        {
            OnSkillShot?.Invoke();
            ElfArrow arrow = PoolManager.Instance.Pop("ElfArrow") as ElfArrow;
            arrow.transform.position = shootPos + new Vector2(_shootPosOffset * Random.Range(-1f, 1f), 0f);
            arrow.Init(Vector2.down, _shootForce, (int)(_towerData.damage*1.5f));
            yield return new WaitForSeconds(_shootDelay);
        }
    }
    protected override void OnThrowTower()
    {
        EventManager.StartListening(Constant.CLICK_SCREEN, UseSkill);
    }
    public override void UseSkill()
    {
        if (gameObject.activeSelf == false) return;
        if (isUsed) return;
        StartCoroutine(ElfTowerSkill());
    }

    public override void DestroyTower()
    {
        EventManager.StopListening(Constant.CLICK_SCREEN, UseSkill);
        base.DestroyTower();
    }

    public override void Reset()
    {
        isUsed = false;
        StopAllCoroutines();
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
}
