using UnityEngine;
using TMPro;
using System.Collections;

public class EntityView : MonoBehaviour
{
    [SerializeField] protected GameObject aimArrow;
    protected EntityController entityController;
    [SerializeField] protected GameObject healthText;
    protected Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        entityController = GetComponent<EntityController>();
        ConnectEvents();
    }

    private void ConnectEvents()
    {
        if (entityController != null)
        {
            entityController.OnActiveturnIndicator += SetActiveTurn;
            entityController.OnDead += DieAnimation;
        }
    }

    protected void disconnectevents()
    {
        entityController.OnActiveturnIndicator -= SetActiveTurn;
        entityController.OnDead -= DieAnimation;
    }

    public void SetHealthText(int amount)
    {
        if (healthText != null)
        {
            healthText.GetComponent<TextMeshProUGUI>().text = amount.ToString();
        }
    }

    public void SetActiveTurn(bool isActive)
    {
        if (aimArrow != null)
        {
            aimArrow.SetActive(isActive);
        }
    }

    public void AttackAnimation()
    {
        animator.SetTrigger("Attack");
    }

    public void DieAnimation()
    {
        animator.SetBool("IsDead", true);
    }
}
