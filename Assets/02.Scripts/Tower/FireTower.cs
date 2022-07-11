using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    public bool isBoom = false;

    [SerializeField] private BoxCollider2D _fireBoomCol;

    protected override void Awake()
    {
        base.Awake();
        _fireBoomCol.enabled = false;
    }

    public override void UseSkill()
    {
        StartCoroutine(StartedMonsterCheck());
    }

    IEnumerator StartedMonsterCheck()
    {
        yield return new WaitForSeconds(1f);
        _fireBoomCol.enabled = true;
        isBoom = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy") && isBoom)
        {
            DestroyTower();
        }
    }
}
