using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PyramidTower : Tower
{
    [SerializeField] private PyramidSwamp _swampObj;

    public UnityEvent OnBrokeTower;


    protected override void Awake()
    {
        base.Awake();
    }

    private IEnumerator SpawnSwamp()
    {
        PyramidSwamp swamp = PoolManager.Instance.Pop("PyramidSwamp") as PyramidSwamp;
        swamp.transform.position = new Vector2(transform.position.x, -7);
        swamp.ParticleStart();
        OnBrokeTower?.Invoke();
        yield return new WaitForSeconds(.01f);
        DestroyTower();

        swamp.gameObject.SetActive(true);
    }

    public override void UseSkill()
    {
        StartCoroutine(SpawnSwamp());
    }

   
}
