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

        Invoke("DestroyTower", 7f);
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
        OnUseSkill?.Invoke();
        _isBoom = true;
        Effect effect = PoolManager.Instance.Pop("ExplosionAnim") as Effect;
        effect.transform.position = transform.position;
        effect.StartAnim();
        DestroyTower();

        yield return null;
    }
    public override void DestroyTower()
    {
        if(!_isBoom)
        {
            CancelInvoke();
            FadeTower(0f);
            StopAllCoroutines();
            Invoke("PushTower", 3f);

            return;
        }
        base.DestroyTower();
    }
#if UNITY_EDITOR
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position, 5f);
    //}
#endif
}
