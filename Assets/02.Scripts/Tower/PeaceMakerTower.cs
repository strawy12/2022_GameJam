using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceMakerTower : Tower
{
    [SerializeField] private BoxCollider2D _peaceCol;

    protected override void Awake()
    {
        base.Awake();
        _peaceCol.enabled = false;
    }

    IEnumerator PeaceMakerAbilityTower()
    {
        Debug.Log("ภ฿ตส");

        GameManager.Inst.isPeaceMonster = true;
        _peaceCol.enabled = true;
        yield return new WaitForSeconds(3f);

        _peaceCol.enabled = false;
        GameManager.Inst.isPeaceMonster = false;
        DestroyTower();
    }

    public override void UseSkill()
    {
        StartCoroutine(PeaceMakerAbilityTower());
    }

}
