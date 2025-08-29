using Assets.Scripts.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }


    [SerializeField] private Camera battleCamera;
    [SerializeField] private GameObject battleHUD;

    private List<EntityController> entities = new List<EntityController>();

    [SerializeField] private GameObject[] playerPjsPos;
    [SerializeField] private GameObject[] enemyPjsPos;

    //ver el tema de niveles de los enemigos
    [SerializeField] private GameObject[] enemyPrefab;

    [SerializeField] private GameObject actionButtons;

    private EntityController activeEntity;
    private EntityController pjObjective;

    private EntityController[] turnOrder;
    private int actualTurnIndex = 0;

    private BattleActions actualAction;

    public Action OnEndedBattle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        AssingButtons();
        AssignEvents();
    }

    private void AssingButtons()
    {
        actionButtons.transform.GetChild(0).GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => ActionSelected(new Attack()));
        actionButtons.transform.GetChild(1).GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => ActionSelected(new Defend()));
    }

    private void AssignEvents()
    {
        GameManager.Instance.OnSetBattlePJ += SetEntitiesInBattlePositions;
        GameManager.Instance.OnStartBattle += StartBattle;
    }

    private void SetEntitiesInBattlePositions(GameObject[] PJs)
    {
        int i = 0;
        if(PJs == null || PJs.Length == 0)
        {
            Debug.LogWarning("No player characters provided for battle.");
            return;
        }
        else
        {
            if(PJs.Any(pj => pj.CompareTag("PlayerPj"))){
                foreach (var pj in PJs)
                {
                    GameObject auxPj = Instantiate(pj, playerPjsPos[i].transform.position, playerPjsPos[i].transform.rotation, playerPjsPos[i].transform);
                    entities.Add(auxPj.GetComponent<EntityController>());
                    i++;
                }
            }
            else
            {
                foreach (var pj in PJs)
                {
                    GameObject auxPj= Instantiate(pj, enemyPjsPos[i].transform.position, enemyPjsPos[i].transform.rotation, enemyPjsPos[i].transform);
                    auxPj.GetComponent<ISetLevel>().OnSetLevel(GameManager.Instance.GetDifficulty().GetEnemyLevel());
                    entities.Add(auxPj.GetComponent<EntityController>());
                    i++;
                }
            }
        }
            
    }

    public void StartBattle()
    {
        CreateEnemies();
        SetOrder();
        ActivateDesactivateCameraAndHUD(true);
        foreach (var player in entities)
        {
            player.OnSelected += SetPjObjective;
        }
        actualTurnIndex = 0;
        activeEntity = turnOrder[actualTurnIndex];
        activeEntity.SetActiveTurnIndicator(true);
        if (actionButtons.CompareTag("PlayerPj"))
        {
            actionButtons.SetActive(true);
        }
    }

    private void ActivateDesactivateCameraAndHUD(bool active)
    {
        battleCamera.gameObject.SetActive(active);
        battleHUD.SetActive(active);
    }

    private bool CheckEndBattle()
    {
        var enemies = entities.Where(p => p.CompareTag("EnemyPj") && p.IsAlive()).ToArray();
        var players = entities.Where(p => p.CompareTag("PlayerPj") && p.IsAlive()).ToArray();

        return enemies.Length == 0 || players.Length == 0;
    }
    public void EndBattle()
    {
        foreach (var pj in entities)
        {
            pj.OnSelected -= SetPjObjective;
        }
        foreach (var entity in enemyPjsPos)
        {
            if(entity.transform.childCount > 0)
            {
                Destroy(entity.transform.GetChild(0).gameObject);
            }
        }
        turnOrder = null;
        actualTurnIndex = 0;
        ActivateDesactivateCameraAndHUD(false);
        OnEndedBattle?.Invoke();
        if(entities.Where(p => p.CompareTag("EnemyPj") && p.IsAlive()).ToArray().Length <= 0)
        {
            float totalExp = entities.Where(p => p.CompareTag("EnemyPj"))
                                      .Sum(p => p.GetComponent<IGiveExperience>().OnGetExperiencePoints());

            foreach (var pj in entities)
            {
                if (pj.CompareTag("PlayerPj") && pj.IsAlive())
                {
                    pj.GetComponent<IGainExperience>().GainExp(totalExp);
                }
            }
        }
        else
        {

        }
        entities.RemoveAll(pj => !pj.CompareTag("PlayerPj"));
    }

    public void SetOrder()
    {
        turnOrder = entities
            .OrderByDescending(p => p.GetSpeed())
            .ThenBy(p => UnityEngine.Random.value)
            .ToArray();
    }

    public void ActionSelected(BattleActions action)
    {
        if (activeEntity.IsAlive())
        {
            actualAction = action;
            actionButtons.SetActive(false);
            foreach (var entity in entities)
            {
                entity.isOnSelectionFase = true;
            }
        }

    }

    public void SetPjObjective(EntityController pj)
    {
        if(pj.CompareTag(activeEntity.tag)) return;
        pjObjective = pj;
        ActionFase();
    }

    private void ActionFase()
    {
        actualAction.Action(pjObjective, activeEntity);
        activeEntity.SetActiveTurnIndicator(false);
        NextTurn();
    }

    public void NextTurn()
    {
        if (CheckEndBattle())
        {
            EndBattle();
            return;
        }
        actualTurnIndex = (actualTurnIndex + 1) % turnOrder.Length;
        activeEntity = turnOrder[actualTurnIndex];
        if(activeEntity.IsAlive() == false)
        {
            NextTurn();
            return;
        }
        pjObjective = null;
        actualAction = null;
        activeEntity.SetActiveTurnIndicator(true);
        foreach (var entity in entities)
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
        EntityController[] pjsPlayer = entities.Where(p => p.CompareTag("PlayerPj") && p.IsAlive()).ToArray();

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
            StartCoroutine(EnemyAction());
        }
    }

    IEnumerator EnemyAction()
    {
        yield return new WaitForSeconds(1f);
        actualAction = new Attack();
        ActionFase();
    }

    private void CreateEnemies()
    {
        //GameObject[] enemies = new GameObject[UnityEngine.Random.Range(1, 5)];
        GameObject[] enemies = new GameObject[UnityEngine.Random.Range(1, 3)];

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Length)];
        }
        SetEntitiesInBattlePositions(enemies);
    }
}
