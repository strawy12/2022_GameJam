using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    public bool _isBoom = false;
    public bool _isCheck = false;
    [SerializeField] private LayerMask _whatIsEnemy;

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
        _isCheck = true;
    }


    protected override void OnTriggerEnemy(Collider2D collision)
    {
        if (!_isStop)
        {
            base.OnTriggerEnemy(collision);
            Debug.Log("hit_Enemy no stop tower");
        }
        else if (_isCheck && !_isBoom)
        {
            StartCoroutine(ExPlosionFireTower());
        }
    }
    IEnumerator ExPlosionFireTower() // 스킬
    {
        _isBoom = true;
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 5f, _whatIsEnemy);

        if (cols.Length >= 2)
        {
            foreach (var hitMonster in cols)
            {
                IHittable hit = hitMonster.GetComponent<IHittable>();
                hit?.GetHit(6, transform.gameObject);
                Debug.Log("boom");
            }
            yield return new WaitForSeconds(0.01f);
            FadeTower(1f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    protected override void SpawnEffect()
    {
    }
#endif
}
