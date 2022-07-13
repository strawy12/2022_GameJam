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
        if (hitCol.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("´ê´Â Áß");

            IHittable hittable = hitCol.GetComponent<IHittable>();
            AgentMovement monsterSpeed = hitCol.GetComponent<AgentMovement>();

            hittable.GetHit(1, transform.gameObject);
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


