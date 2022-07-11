using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower : Tower
{
    private BoxCollider2D _exploreCol;

    protected override void Awake()
    {
        base.Awake();
        anim.SetBool("isExplore", false);

        _exploreCol = GetComponent<BoxCollider2D>();
    }

    IEnumerator ExPlosionStoneTower()
    {
        anim.SetBool("isExplore", true);
        yield return new WaitForSeconds(1f);
        
        DestroyTower();
    }
}
