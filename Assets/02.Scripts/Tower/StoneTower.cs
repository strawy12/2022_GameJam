using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoneTower : Tower
{
    //[SerializeField] private Animator anim;
    [SerializeField] private LayerMask _whatIsEnemy;

    public UnityEvent OnBrokeTower;

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
        OnBrokeTower?.Invoke();
        yield return new WaitForSeconds(0.01f);

        DestroyTower();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }
#endif
}
