using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private PlayerController playerController;

    private Difficulty difficulty;

    public Action<GameObject[]> OnSetBattlePJ;
    public Action OnStartBattle;
    public Action OnEndBattle;

    public Action OnStartExploring;
    public Action OnEndExploring;

    //TO DO: despues cambiar el lugar donde esta el manejo de las interfaces
    [SerializeField] private GameObject preparationMenu;

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

    private void OnEnable()
    {
        AssingButtons();
        AssignEvents();
    }

    private void AssingButtons()
    {
        preparationMenu.transform.GetChild(1).GetChild(0).GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => StartExploring(new EasyDifficulty()));
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

    public void StartExploring(Difficulty explorationDifficulty)
    {
        difficulty = explorationDifficulty;
        preparationMenu.SetActive(false);
        OnStartExploring?.Invoke();
    }

    public void EndExploring()
    {
        OnEndExploring?.Invoke();
        preparationMenu.SetActive(true);
    }

    public Difficulty GetDifficulty() { return difficulty; }
}
