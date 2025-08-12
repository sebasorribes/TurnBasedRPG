using UnityEngine;

[CreateAssetMenu(fileName = "BaseStats", menuName = "ScriptableObjects/BaseStats", order = 1)]
public class ScriptableBaseStats : ScriptableObject
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxMana;
    [SerializeField] private int attackStat;
    [SerializeField] private int defenseStat;
    [SerializeField] private int speedStat;
    [SerializeField] private int accuracyStat;

    public float MaxHealth => maxHealth;
    public float MaxMana => maxMana;
    public int AttackStat => attackStat;
    public int DefenseStat => defenseStat;
    public int SpeedStat => speedStat;
    public int AccuracyStat => accuracyStat;

}
