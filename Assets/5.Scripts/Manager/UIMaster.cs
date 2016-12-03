using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIMaster : BaseManager {

    public GameObject gameOverPanel;

    public override void Init()
    {
        
    }

    public override void DoUpdate()
    {
        
    }

    public void RestartGame()
    {
        //Application.LoadLevel(0);
        SceneManager.LoadScene(0);
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }
}
