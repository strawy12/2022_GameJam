using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PeaceMakerTower : Tower
{ 
    public bool isSkill = false;

    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private GameObject _skillEffect;
    [SerializeField] private float _skillDuration = 3f; 
    public UnityEvent OnUsedSkill;

    protected override void Awake()
    {
        base.Awake();
    }

    private void FixedUpdate()
    {
        if(isSkill)
        {
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 3f, _whatIsEnemy);

            foreach (var hitMonster in cols)
            {
                AgentMovement monsterAgent = hitMonster.GetComponent<AgentMovement>();
                monsterAgent.StopImmediatelly();
            }
        }
    }

    private IEnumerator PeaceMakerAbilityTower()
    {
        isSkill = true;
        OnUsedSkill?.Invoke();
        yield return new WaitForSeconds(3f);
        isSkill = false;
        DestroyTower();
    }

    public override void UseSkill()
    {
        StartCoroutine(PeaceMakerAbilityTower());
        Effect effect = PoolManager.Instance.Pop(_skillEffect.gameObject.name) as Effect;
        effect.transform.position = transform.position;
        effect.SetLifeTime(_skillDuration);
    }

#if UNITY_EDITOR
    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
        base.OnDrawGizmos();
    }
#endif
}

