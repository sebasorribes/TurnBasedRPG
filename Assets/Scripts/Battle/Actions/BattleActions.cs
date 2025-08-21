using UnityEngine;

public abstract class BattleActions : MonoBehaviour
{
    public abstract void Action(EntityController target, EntityController attacker);
}
