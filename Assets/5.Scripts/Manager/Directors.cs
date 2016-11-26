using UnityEngine;
using System.Collections;

public class Directors : MonoBehaviour {

    public static CameraController cameraController;
    public static MouseController mouseController;

    public static PlayerManager playerManager;
    public static EnemyManager enemyManager;

    //Later if I want to change to use fixed deltaTime or anything, I can just change it from here.
    private static float deltaTime;
    public static float GetDeltaTime() { return deltaTime; }

    void Awake()
    {
        PrepareManagers();
        InitManagers();
    }

    void Update()
    {
        deltaTime = Time.deltaTime;
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
    }

    private void InitManagers()
    {
        playerManager.Init();
        enemyManager.Init();
        cameraController.Init();
        mouseController.Init();
    }

    private void UpdateManagers()
    {
        mouseController.DoUpdate();
        cameraController.DoUpdate();
        playerManager.DoUpdate();
        enemyManager.DoUpdate();
    }

}
