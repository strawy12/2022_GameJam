using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoneTower : Tower
{

    [SerializeField] private LayerMask _whatIsEnemy;


    public override void UseSkill()
    {
        StartCoroutine(ExPlosionStoneTower());
    }

    private IEnumerator ExPlosionStoneTower()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2.5f, _whatIsEnemy);

        foreach (var hitMonster in cols)
        {
            IHittable hit = hitMonster.GetComponent<IHittable>();
            hit?.GetHit((int)_towerData.damage, transform.gameObject);
        }

        yield return null;

        DestroyTower();
    }

#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, 2.5f);
    //}
#endif
}
