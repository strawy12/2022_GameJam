using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidSwamp : PoolableMono
{
    [SerializeField] private LayerMask _whatIsEnemy;

    private void OnEnable()
    {
        StartCoroutine(MakePyramidSwamp());
    }

    private void OnTriggerStay2D(Collider2D hitCol)
    {
        if(hitCol.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IHittable monsterHit = hitCol.GetComponent<IHittable>();
            AgentMovement monsterSpeed = hitCol.GetComponent<AgentMovement>();

            monsterHit.GetHit(1, transform.gameObject);
            monsterSpeed.SwampStateEnemyRun();
        }
    }

    IEnumerator MakePyramidSwamp()
    {
        yield return new WaitForSeconds(5f);
        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
        if (_whatIsEnemy != LayerMask.NameToLayer("Enemy"))
            _whatIsEnemy = LayerMask.NameToLayer("Enemy");
    }
}


