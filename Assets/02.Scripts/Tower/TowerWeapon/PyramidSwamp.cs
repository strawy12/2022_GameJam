using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidSwamp : PoolableMono
{
    [SerializeField] private LayerMask _whatIsEnemy;

    public void ParticleStart() 
    { 
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
        if (_whatIsEnemy != LayerMask.NameToLayer("Enemy"))
            _whatIsEnemy = LayerMask.NameToLayer("Enemy");
    }
}


