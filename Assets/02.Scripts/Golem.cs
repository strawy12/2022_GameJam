using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Golem : MonoBehaviour,IHittable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Animator golemHitAnim;
    
    public int Health { get; private set; }

    public UnityEvent OnPlayerDead;
    public UnityEvent OnHit;
    private bool _isDead =false;

    private void Awake()
    {
        OnPlayerDead.AddListener(UIManager.Inst.GoUpgradeUI);
    }
    public void GetHit(int damage, GameObject damageDealer)
    {
        golemHitAnim?.Play("GolemHit");
        Health -= damage;
        OnHit?.Invoke();
        if (Health <= 0)
        {
            Death();
        }
    }
    private void Start()
    {
        Health = maxHealth;
    }
    public void ResetHP()
    {
        Health = maxHealth;
    }

    public void Death()
    {
        _isDead = true;
        OnPlayerDead?.Invoke();
    }
}
