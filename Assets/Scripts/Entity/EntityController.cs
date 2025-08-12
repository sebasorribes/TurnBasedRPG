using System;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [SerializeField] protected ScriptableBaseStats baseStats;

    protected EntityModel model;
    protected EntityView view;

    public bool isOnSelectionFase=false;

    public Action OnDead;
    public Action<bool> OnActiveturnIndicator;
    public Action<EntityController> OnSelected;

    protected void Awake()
    {
        model = GetComponent<EntityModel>();
        view = GetComponent<EntityView>();
        model.OnDie += Die;
        Build();
    }

    protected void Build()
    {
        model.BuildModel(baseStats);
        view.SetHealthText((int)model.GetCurrentHealth());
    }

    public void TakeDamage(float amount)
    {
        model.TakeDamage(amount);
        view.SetHealthText((int)model.GetCurrentHealth());
    }

    public void Die()
    {
        OnDead?.Invoke();
    }

    public void Heal(float amount)
    {
    }

    public int Attack()
    {
        view.AttackAnimation();
        return model.GetAttackStat();
    }

    public void SpecialAttack()
    {

    }

    public void LevelUp()
    {
        model.LevelUp();
    }

    public float GetCurrentHealth()
    {
        return model.GetCurrentHealth();
    }

    public float GetSpeed()
    {
        return model.GetSpeed();
    }

    private void OnMouseDown()
    {
        if(isOnSelectionFase) OnSelected?.Invoke(this);
    }

    public void SetActiveTurnIndicator(bool isActive)
    {
        OnActiveturnIndicator?.Invoke(isActive);
    }

    public bool IsAlive()
    {
        return model.GetIsAlive();
    }
}
