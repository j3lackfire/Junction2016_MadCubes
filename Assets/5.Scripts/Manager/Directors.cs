using UnityEngine;
using System.Collections.Generic;

public class Directors : MonoBehaviour {

    private static Directors _instance;
    public static Directors instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }

    public List<BaseManager> managersList = new List<BaseManager>();

    public CameraController cameraController;
    public MouseController mouseController;

    public PlayerManager playerManager;
    public EnemyManager enemyManager;

    public ProjectileManager projectileManager;

    private BattleState battleState;

    private void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<Directors>();
        } else
        {
            if (instance.gameObject == null)
            {
                instance = FindObjectOfType<Directors>();
            } else
            {
                Destroy(this.gameObject);
                return;
            }
        } 
        DontDestroyOnLoad(this.gameObject);

        battleState = BattleState.Prepare;
        PrefabsManager.ClearPool();
        PrepareManagers();
        InitManagers();
    }

    private void Update()
    {
        UpdateManagers();
    }

    public BattleState GetBattleState() { return battleState; }

    public void StartBattle()
    {
        battleState = BattleState.Battling;
        Debug.Log("Start battle !!! " + battleState);
    }

    public void EndBattle()
    {
        battleState = BattleState.Finish;
    }

    private void PrepareManagers()
    {
        PlayerManager tempObjectManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            Destroy(playerManager.gameObject);
        }
        playerManager = tempObjectManager;

        EnemyManager tempEnemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager != null)
        {
            Destroy(enemyManager.gameObject);
        }
        enemyManager = tempEnemyManager;

        CameraController tempCameraController = FindObjectOfType<CameraController>();
        if (cameraController != null)
        {
            Destroy(cameraController.gameObject);
        }
        cameraController = tempCameraController;

        MouseController tempMouseController = FindObjectOfType<MouseController>();
        if (mouseController != null)
        {
            Destroy(mouseController.gameObject);
        }
        mouseController = tempMouseController;

        ProjectileManager tempProjectileManager = FindObjectOfType<ProjectileManager>();
        if (projectileManager != null)
        {
            Destroy(projectileManager.gameObject);
        }
        projectileManager = tempProjectileManager;


        managersList = new List<BaseManager>();
        managersList.Add(cameraController);
        managersList.Add(mouseController);
        managersList.Add(enemyManager);
        managersList.Add(playerManager);
        managersList.Add(projectileManager);
    }

    private void InitManagers()
    {
        for (int i = 0; i < managersList.Count; i ++)
        {
            managersList[i].Init();
        }
    }

    private void UpdateManagers()
    {
        for (int i = 0; i < managersList.Count; i++)
        {
            managersList[i].DoUpdate();
        }
    }

}

public enum BattleState
{
    Invalid = -1,
    Prepare = 0,
    Battling = 1,
    Finish = 2
}