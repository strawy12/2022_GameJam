using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Golem : MonoBehaviour,IHittable
{
    [SerializeField] private BarUI _hpBar;
    
    public int Health { get; private set; }
    public int MaxHealth => (int)DataManager.Inst.CurrentPlayer.GetStat(PlayerStatData.EPlayerStat.MaxHp);

    public UnityEvent OnPlayerDead;
    public UnityEvent OnHit;
    private bool _isDead =false;

    private void Awake()
    {
        OnPlayerDead.AddListener(UIManager.Inst.GoUpgradeUI);
    }
    public void GetHit(int damage, GameObject damageDealer)
    {
        Health -= damage;
        OnHit?.Invoke();
        _hpBar.SetGuageUI((float)Health / MaxHealth);
        if (Health <= 0)
        {
            Death();
        }
    }
    private void Start()
    {

        Health = MaxHealth;
        _hpBar.SetGuageUI((float)Health / MaxHealth);
    }
    public void ResetHP()
    {
        Health = MaxHealth;
        _hpBar.SetGuageUI((float)Health / MaxHealth);
    }

    public void Death()
    {
        _isDead = true;
        OnPlayerDead?.Invoke();
    }
}
