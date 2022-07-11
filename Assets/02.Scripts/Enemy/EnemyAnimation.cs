using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimation : MonoBehaviour
{
    protected readonly int _attackHashStr = Animator.StringToHash("Attack");
    protected readonly int _deathHashStr = Animator.StringToHash("Dead");
    protected readonly int _walkHashStr = Animator.StringToHash("Walk");

    protected Animator _agentAnimator;
    protected EnemyAIBrain _enemyAIBrain;
    protected virtual void Awake()
    {
        _enemyAIBrain = transform.parent.GetComponent<EnemyAIBrain>();
        _agentAnimator = GetComponent<Animator>();
    }
    public void PlayAttackAnimation()
    {
        _agentAnimator.SetTrigger(_attackHashStr);
    }
    public void SetWalkAnimation(bool value)
    {
        _agentAnimator.SetBool(_walkHashStr, value);
    }
    public void AnimatePlayer(float velocity)
    {
        SetWalkAnimation(velocity > 0);
    }
    public void PlayDeathAnimation()
    {
        _agentAnimator.SetTrigger(_deathHashStr);
    }

}
