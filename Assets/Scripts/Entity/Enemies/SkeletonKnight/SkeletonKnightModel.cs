using Assets.Scripts.Interfaces;
using UnityEngine;

public class SkeletonKnightModel : EntityModel, ISetLevel
{

    public void OnSetLevel(int level)
    {
        this.level = level;
        UpStats();
    }


    //balancear
    protected override void UpStats()
    {
        maxHealth = (int)(baseStats.MaxHealth + (level * 30));
        currentHealth = maxHealth;
        maxMana = (int)(baseStats.MaxMana + (level * 10));
        currentMana = maxMana;
        attackStat = (int)(baseStats.AttackStat + (level * 1.5f));
        defenseStat = (int)(baseStats.DefenseStat + (level * 2));
        speedStat = (int)(baseStats.SpeedStat + (level * 1.2f));
    }
}
