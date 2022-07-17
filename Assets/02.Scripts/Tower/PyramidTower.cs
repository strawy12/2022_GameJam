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
        swamp.transform.position = new Vector2(transform.position.x, -7);
        swamp.ParticleStart();
        yield return new WaitForSeconds(.01f);
        DestroyTower();

        swamp.gameObject.SetActive(true);
    }

    public override void UseSkill()
    {
        StartCoroutine(SpawnSwamp());
    }

   
}
