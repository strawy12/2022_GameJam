using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Golem : MonoBehaviour,IHittable
{
    [SerializeField] private int maxHealth;
    public int Health { get; private set; }

    public UnityEvent OnPlayerDead;
    private bool _isDead =false;

    private void Awake()
    {
        OnPlayerDead.AddListener(UIManager.Inst.GoUpgradeUI);
    }
    public void GetHit(int damage, GameObject damageDealer)
    {
        Health -= damage;
        if(Health <= 0)
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
