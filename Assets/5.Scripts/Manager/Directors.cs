using UnityEngine;
using System.Collections;

public class Directors : MonoBehaviour {

    public static CameraController cameraController;
    public static MouseController mouseController;

    public static PlayerManager playerManager;
    public static EnemyManager enemyManager;

    public static UIMaster uiMaster;

    public static ProjectileManager projectileManager;

    void Awake()
    {
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

        UIMaster tempUI = FindObjectOfType<UIMaster>();
        if (uiMaster != null)
        {
            Destroy(uiMaster.gameObject);
        }
        uiMaster = tempUI;


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

    }

    private void InitManagers()
    {
        playerManager.Init();
        enemyManager.Init();
        cameraController.Init();
        mouseController.Init();
        projectileManager.Init();
        uiMaster.Init();
    }

    private void UpdateManagers()
    {
        mouseController.DoUpdate();
        cameraController.DoUpdate();
        playerManager.DoUpdate();
        enemyManager.DoUpdate();
        projectileManager.DoUpdate();
        uiMaster.DoUpdate();
    }

}
