using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidTower : Tower
{

   private BoxCollider2D _attackCol;

    protected override void Awake()
    {
        base.Awake();
        anim.SetBool("isExplore", false);

        _attackCol = GetComponent<BoxCollider2D>();
    }

    IEnumerator ExPlosionEffect()
    {
        anim.SetBool("isExplore", true);
        yield return new WaitForSeconds(1f);
        
        DestroyTower();
    }
}
