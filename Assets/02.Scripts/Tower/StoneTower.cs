using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower : Tower
{
    [SerializeField] private Animator anim;
    [SerializeField] private BoxCollider2D _exploreCol;

    protected override void Awake()
    {
        base.Awake();
        anim.SetBool("isExplore", false);
        _exploreCol.enabled = false;
    }

    IEnumerator ExPlosionStoneTower()
    {
        anim.SetBool("isExplore", true);
        _exploreCol.enabled = true;
        yield return new WaitForSeconds(1f);
        
        DestroyTower();
    }

    public override void UseSkill()
    {
        StartCoroutine(ExPlosionStoneTower());
    }
}
