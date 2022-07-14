using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireTower : Tower
{
    public bool _isBoom = false;
    public bool _isCheck = false;
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
        _isCheck = true;
        OnUseSkill?.Invoke();

    }


    protected override void OnTriggerEnemy(Collider2D collision)
    {
        if (!_isStop)
        {
            base.OnTriggerEnemy(collision);
        }
        else if (_isCheck && !_isBoom)
        {
            EventManager<Vector3>.TriggerEvent(Constant.TOWER_BOOM, transform.position);
            StartCoroutine(ExPlosionFireTower());
        }
    }

    private IEnumerator ExPlosionFireTower()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 5f, _whatIsEnemy);

        foreach (var hitMonster in cols)
        {
            IHittable hit = hitMonster.GetComponent<IHittable>();
            hit?.GetHit((int)_towerData.damage, transform.gameObject);
        }
        yield return new WaitForSeconds(0.01f);
        _isBoom = true;
        Effect effect = PoolManager.Instance.Pop("ExplosionAnim") as Effect;
        effect.transform.position = transform.position;
        effect.StartAnim();
        DestroyTower();
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
#endif
}
