using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerController playerController;

    public Action<GameObject[]> OnSetBattlePJ;
    public Action OnStartBattle;
    public Action OnEndBattle;

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
        AssignEvents();
    }

    private void AssignEvents()
    {
        playerController.OnStartBattle += StartBattle;
        playerController.OnSetBattlePJ += SetPlayerPjs;
        BattleManager.Instance.OnEndedBattle += EndBattle;
    }

    private void UnassignEvents()
    {
        playerController.OnStartBattle -= StartBattle;
        playerController.OnSetBattlePJ -= SetPlayerPjs;
        BattleManager.Instance.OnEndedBattle -= EndBattle;
    }

    private void SetPlayerPjs(GameObject[] pjs)
    {
        OnSetBattlePJ?.Invoke(pjs);
    }

    private void StartBattle()
    {
        OnStartBattle?.Invoke();
    }

    private void EndBattle()
    {
        OnEndBattle?.Invoke();
    }
}
