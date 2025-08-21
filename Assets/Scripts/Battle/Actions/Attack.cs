using System;
using UnityEngine;

public class Attack : BattleActions
{
    public override void Action(EntityController target, EntityController attacker)
    {
        target.TakeDamage(attacker.Attack());
    }
}
