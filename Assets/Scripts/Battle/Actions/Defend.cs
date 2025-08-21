using UnityEngine;

public class Defend : BattleActions
{
    public override void Action(EntityController target, EntityController attacker)
    {
        Debug.Log("Defend action Selected to " + attacker);
    }
}
