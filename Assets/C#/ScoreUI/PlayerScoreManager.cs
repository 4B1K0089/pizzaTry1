using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance;

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    public int WinningScore = 3;

    [Header("Audio")]
    public AudioClip scoreClip; // 拖進得分音效
    public AudioSource audioSource; // 拖進一個 AudioSource（建議同一個物件上）

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int playerId, int amount)
    {
        if (!playerScores.ContainsKey(playerId))
            playerScores[playerId] = 0;

        playerScores[playerId] += amount;
        Debug.Log($"P{playerId} 得到 1 分，現在是 {playerScores[playerId]} 分");

        // ? 播放得分音效
        if (scoreClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(scoreClip);
        }

        UIManager.Instance.UpdateScore(playerId, playerScores[playerId]);

        if (playerScores[playerId] >= WinningScore)
        {
            Debug.Log($"?? P{playerId} 獲勝！");
            GameOverUIManager.Instance.ShowGameOver(playerScores, playerId);
        }
    }

    public int GetScore(int playerId)
    {
        return playerScores.ContainsKey(playerId) ? playerScores[playerId] : 0;
    }
}
