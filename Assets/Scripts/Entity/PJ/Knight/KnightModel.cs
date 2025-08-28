using Assets.Scripts.Interfaces;
using UnityEngine;

public class KnightModel : EntityModel, IGainExperienceModel
{
    [SerializeField] private float currentExp = 100f;
    [SerializeField] private float baseExp = 100f;
    [SerializeField] private float expGrowth = 1.5f;

    public override void BuildModel(ScriptableBaseStats baseStats)
    {
        base.BuildModel(baseStats);
        currentExp = 0;
    }

    protected override void UpStats()
    {
        maxHealth = (int) (baseStats.MaxHealth + (level * 30));
        maxMana = (int) (baseStats.MaxMana + (level * 10));
        attackStat = (int) (baseStats.AttackStat + (level * 1.5f));
        defenseStat = (int) (baseStats.DefenseStat + (level * 2));
        speedStat = (int) (baseStats.SpeedStat + (level * 1.2f));

    }

    public void GainExp(float amount)
    {
        currentExp += amount;
        float expToNext = CalculateExpToNextlevel();

        while (currentExp >= expToNext)
        {
            currentExp -= expToNext;
            LevelUp();
            expToNext = CalculateExpToNextlevel();
        }
    }

    //ver como pasarlo a la interfaz pero que se pueda modificar el nivel del enemigo
    public void LevelUp()
    {
        level++;
        UpStats();
    }

    public float CalculateExpToNextlevel()
    {
        return baseExp * Mathf.Pow(level, expGrowth);
    }
}
