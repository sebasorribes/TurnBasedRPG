using System.Collections;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private EntityController[] players;
    [SerializeField] private GameObject actionButtons;
    private EntityController activeEntity;
    private EntityController pjObjective;
    private string actionSelected;
    private EntityController[] turnOrder;
    private int actualTurnIndex = 0;

    private void Start()
    {
        StartBattle();
    }
    public void StartBattle()
    {
        foreach (var player in players)
        {
            player.OnSelected += SetPjObjective;
        }
        SetOrder();
        actualTurnIndex = 0;
        activeEntity = turnOrder[actualTurnIndex];
        activeEntity.SetActiveTurnIndicator(true);
        if (actionButtons.CompareTag("PlayerPj"))
        {
            actionButtons.SetActive(true);
        }
    }
    public void EndBattle()
    {
        Debug.Log("Battle Ended");
        actualTurnIndex = 0;
    }

    public void SetOrder()
    {
        turnOrder = players
            .OrderByDescending(p => p.GetSpeed())
            .ThenBy(p => UnityEngine.Random.value)
            .ToArray();
    }

    public void ActionSelected(string action)
    {
        if (activeEntity.IsAlive())
        {
            actionSelected = action;
            actionButtons.SetActive(false);
            foreach (var entity in players)
            {
                entity.isOnSelectionFase = true;
            }
        }
        
    }

    public void Attack()
    {
        pjObjective.TakeDamage(activeEntity.Attack());
        activeEntity.SetActiveTurnIndicator(false);
        NextTurn();
    }

    public void SetPjObjective(EntityController pj)
    {
        if(pj.CompareTag(activeEntity.tag)) return;
        pjObjective = pj;
        Debug.Log(pjObjective);
        ActionFase();
    }

    private void ActionFase()
    {
        switch(actionSelected)
        {
            case "Attack":
                Attack();
                break;
            default:
                Debug.Log("No action selected or invalid action.");
                break;
        }
    }

    public void NextTurn()
    {
        actualTurnIndex = (actualTurnIndex + 1) % turnOrder.Length;
        activeEntity = turnOrder[actualTurnIndex];
        if(activeEntity.IsAlive() == false)
        {
            NextTurn();
            return;
        }
        pjObjective = null;
        activeEntity.SetActiveTurnIndicator(true);
        foreach (var entity in players)
        {
            entity.isOnSelectionFase = false;
        }
        if(activeEntity.CompareTag("PlayerPj"))
        {
            actionButtons.SetActive(true);
        }else{
            EnemyTurn();
        }
        
    }

    private void EnemyTurn()
    {
        EntityController[] pjsPlayer = players.Where(p => p.CompareTag("PlayerPj") && p.IsAlive()).ToArray();

        if(pjsPlayer.Length == 0)
        {
            Debug.Log("All player characters are dead. Game Over.");
            EndBattle();
            return;
        }
        else
        {
            int randomIndex = UnityEngine.Random.Range(0, pjsPlayer.Length);
            pjObjective = pjsPlayer[randomIndex];
            Debug.Log($"Enemy {activeEntity.name} attacks {pjObjective.name}");
            StartCoroutine(EnemyAction());
        }
    }

    IEnumerator EnemyAction()
    {
        yield return new WaitForSeconds(1f);
        Attack();
    }
}
