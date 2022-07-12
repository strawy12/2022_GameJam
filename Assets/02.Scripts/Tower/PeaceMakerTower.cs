using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceMakerTower : Tower
{
    //[SerializeField] private BoxCollider2D _peaceCol;

    protected override void Awake()
    {
        base.Awake();
    }

    IEnumerator PeaceMakerAbilityTower()
    {
        Debug.Log("ภ฿ตส");

        yield return new WaitForSeconds(3f);

        DestroyTower();
    }

    public override void UseSkill()
    {
        StartCoroutine(PeaceMakerAbilityTower());
    }

}
