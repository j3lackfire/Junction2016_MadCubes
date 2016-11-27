using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMaster : MonoBehaviour {

    public GameObject gameOverPanel;

    public void RestartGame()
    {
        Application.LoadLevel(0);
    }

    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
    }
}
