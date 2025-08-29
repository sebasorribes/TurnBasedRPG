using System;
using UnityEngine;

public class EntityModel : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    protected float maxMana;
    protected float currentMana;

    protected int level;

    protected ScriptableBaseStats baseStats;

    protected int attackStat;
    protected int defenseStat;
    protected int speedStat;
    protected int accuracyStat;

    protected bool isAlive;
    public Action OnDie;

    public void TakeDamage(float amount)
    {
        float aux = Mathf.Max(amount - defenseStat,0);
        currentHealth = Mathf.Max(currentHealth - aux, 0);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public int GetAttackStat()
    {
        return attackStat;
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetSpeed()
    {
        return speedStat;
    }

    public int GetLevel()
    {
        return level;
    }
    protected void Die()
    {
        isAlive = false;
        OnDie?.Invoke();
    }

    public virtual void BuildModel(ScriptableBaseStats baseStats)
    {
        this.baseStats = baseStats;

        maxHealth = baseStats.MaxHealth;
        currentHealth = maxHealth;
        maxMana = baseStats.MaxMana;
        currentMana = maxMana;

        level = 1;

        isAlive = true;
         
        attackStat = baseStats.AttackStat;
        defenseStat = baseStats.DefenseStat;
        speedStat = baseStats.SpeedStat;
        accuracyStat = baseStats.AccuracyStat;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    protected virtual void UpStats()
    {

    }
}
