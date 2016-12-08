using UnityEngine;
using System.Collections.Generic;

public class Directors : MonoBehaviour {

    public static List<BaseManager> managersList = new List<BaseManager>();

    public static CameraController cameraController;
    public static MouseController mouseController;

    public static PlayerManager playerManager;
    public static EnemyManager enemyManager;

    public static ProjectileManager projectileManager;

    void Awake()
    {
        PrefabsManager.ClearPool();
        PrepareManagers();
        InitManagers();
    }

    void Update()
    {
        UpdateManagers();
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
