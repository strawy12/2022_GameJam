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
        FadeTower(0.1f);

        swamp.gameObject.SetActive(true);

        //deletePyramidTower.Play("PyramidDust");
   

    }

    public override void UseSkill()
    {
        StartCoroutine(SpawnSwamp());
    }

    protected override void SpawnEffect()
    {
        Vector2 rayPos = transform.position;
        rayPos.y = 10f;
        var hit = Physics2D.Raycast(rayPos, Vector2.down, 999f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(rayPos, Vector2.down * 999f, Color.red, 10f);
        if (hit.collider != null)
        {
            Effect effect = PoolManager.Instance.Pop(_effectPrefab.name) as Effect;
            effect.transform.SetPositionAndRotation(hit.point, Quaternion.identity);
            effect.StartAnim();
        }
        
    }
}
