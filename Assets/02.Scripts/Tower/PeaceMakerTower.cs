using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceMakerTower : Tower
{
    private BoxCollider2D _peaceCol;

    protected override void Awake()
    {
        base.Awake();

        _peaceCol = GetComponent<BoxCollider2D>();
    }

    IEnumerator PeaceMakerAbilityTower()
    {
        GameManager.Inst.isPeaceMonster = true;
        yield return new WaitForSeconds(3f);

        GameManager.Inst.isPeaceMonster = false;
        DestroyTower();
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(PeaceMakerAbilityTower());
    }
}
