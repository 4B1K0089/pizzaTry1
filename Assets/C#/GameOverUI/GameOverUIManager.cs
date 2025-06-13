using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance;

    public GameObject gameOverPanel;
    public TMP_Text resultsText;
    public Image[] winImages; // [0] = P1勝利圖, [1] = P2, etc.

    private bool inputEnabled = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowGameOver(Dictionary<int, int> playerScores, int winnerId)
    {
        gameOverPanel.SetActive(true);
        ShowSortedScores(playerScores);
        ShowWinImage(winnerId);
        inputEnabled = true;
    }

    void ShowSortedScores(Dictionary<int, int> playerScores)
    {
        List<KeyValuePair<int, int>> sorted = new List<KeyValuePair<int, int>>(playerScores);
        sorted.Sort((a, b) => b.Value.CompareTo(a.Value)); // 降序排序

        string result = "分數結果：\n";
        foreach (var entry in sorted)
        {
            result += $"P{entry.Key}: {entry.Value} 分\n";
        }

        //resultsText.text = result;
    }

    void ShowWinImage(int winnerId)
    {
        for (int i = 0; i < winImages.Length; i++)
        {
            winImages[i].gameObject.SetActive(i == winnerId - 1); // 因為winImages[0] = P1
        }
    }

    private void Update()
    {
        if (!inputEnabled) return;

        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame) // A鍵
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
            else if (Gamepad.current.buttonEast.wasPressedThisFrame) // B鍵
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }
        }
    }
}
