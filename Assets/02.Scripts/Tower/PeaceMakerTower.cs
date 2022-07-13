using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceMakerTower : Tower
{
    public bool isSkill = false;

    [SerializeField] private LayerMask _whatIsEnemy;

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
                Debug.Log("����");
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

        DestroyTower();
    }

    public override void UseSkill()
    {
        StartCoroutine(PeaceMakerAbilityTower());
    }

    private void ColiderCheck()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 5f);
    }
#endif
}

