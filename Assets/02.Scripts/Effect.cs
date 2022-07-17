using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : PoolableMono
{
    [SerializeField] private AnimationClip _animationClip;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void StartAnim()
    {
        _animator.Play(_animationClip.name);
    }
    
    public void EndAnim()
    {
        PoolManager.Instance.Push(this);
    }
    public void SetLifeTime(float duration)
    {
        Invoke("EndAnim", duration);
        StartAnim();
    }
    public override void Reset()
    {

    }
}
