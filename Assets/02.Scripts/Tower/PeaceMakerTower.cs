using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceMakerTower : Tower
{
    public bool isSkill = false;

    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private Animator deletePeaceMakerTower;

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
                Debug.Log("∏ÿ√„");
                //EnemyAIBrain monsterAI = hitMonster.GetComponent<EnemyAIBrain>();

                monsterAgent.StopImmediatelly();
            }
        }
    }

    IEnumerator PeaceMakerAbilityTower()
    {
        isSkill = true;
        yield return new WaitForSeconds(3f);

        isSkill = false;
        deletePeaceMakerTower.Play("PeaceMakerDust");
        yield return new WaitForSeconds(0.5f);

        DestroyTower();
    }

    public override void UseSkill()
    {
        StartCoroutine(PeaceMakerAbilityTower());
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }

    protected override void SpawnEffect()
    {
        throw new System.NotImplementedException();
    }
#endif
}

