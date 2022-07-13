using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireTower : Tower
{
    public bool isBoom = false;

    [SerializeField] private LayerMask _whatIsEnemy;

    public UnityEvent OnUseSkill;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void UseSkill()
    {
        StartCoroutine(StartedMonsterCheck());
    }

    IEnumerator StartedMonsterCheck() // 1초뒤 시작되게
    {
        yield return new WaitForSeconds(1f);
        isBoom = true;
        OnUseSkill?.Invoke();
        //Debug.Log("일단 스타트몬스터쳌");
    }

    IEnumerator ExPlosionFireTower() // 스킬
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 5f, _whatIsEnemy);

        if (cols.Length >= 2)
        {
            foreach (var hitMonster in cols)
            {
                IHittable hit = hitMonster.GetComponent<IHittable>();
                hit?.GetHit(6, transform.gameObject);
            }

            yield return new WaitForSeconds(0.5f);
            DestroyTower();
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
#endif
}
