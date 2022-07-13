using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneTower : Tower
{
    [SerializeField] private Animator anim;
    [SerializeField] private LayerMask _whatIsEnemy;

    protected override void Awake()
    {
        base.Awake();
        anim.SetBool("isExplore", false);
    }

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
            hit?.GetHit(4, transform.gameObject);
        }

        yield return new WaitForSeconds(1f);
        FadeTower(1f);
    }

    protected override void SpawnEffect()
    {
        Vector2 rayPos = transform.position;
        rayPos.y = 10f;
        var hit = Physics2D.Raycast(rayPos, Vector2.down, 999f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(rayPos, Vector2.down * 999f, Color.red, 10f);
        if (hit.collider != null)
        {
            Effect effect = PoolManager.Instance.Pop(_effectPrefab.name) as Effect;
            effect.transform.SetPositionAndRotation(hit.point, Quaternion.identity);
            effect.StartAnim();
        }

        ShakeObject(hit.point);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 2.5f);
    }

    
#endif
}
