using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidSwamp : PoolableMono
{
    private int _damage;
    public void SkillStart(int damage ) 
    {
        _damage = damage;

        StartCoroutine(MakePyramidSwamp());
    }

    private void OnTriggerStay2D(Collider2D hitCol)
    {
        if (hitCol.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            IHittable hittable = hitCol.GetComponent<IHittable>();
            AgentMovement monsterSpeed = hitCol.GetComponent<AgentMovement>();
            hittable.GetHit(1, transform.gameObject);
            monsterSpeed.SwampStateEnemyRun(0.2f, 3f);
        }
    }

    IEnumerator MakePyramidSwamp()
    {
        yield return new WaitForSeconds(3f);
        PoolManager.Instance.Push(this);
    }

    public override void Reset()
    {
    }
}


