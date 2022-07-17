using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyramidTower : Tower
{
    [SerializeField] private PyramidSwamp _swampObj;

    protected override void Awake()
    {
        base.Awake();
    }

    private IEnumerator SpawnSwamp()
    {
        PyramidSwamp swamp = PoolManager.Instance.Pop("PyramidSwamp") as PyramidSwamp;

        swamp.transform.position = new Vector2(transform.position.x, GetGroundPos().y);
        swamp.SkillStart((int)_towerData.damage);

        yield return null;

        DestroyTower();

        swamp.gameObject.SetActive(true);
    }

    private Vector2 GetGroundPos()
    {
       var hit = Physics2D.Raycast(transform.position + Vector3.up * 10f, Vector3.down, 999f, LayerMask.GetMask("Ground"));

        return hit.point;
    }

    public override void UseSkill()
    {
        StartCoroutine(SpawnSwamp());
    }

   
}
