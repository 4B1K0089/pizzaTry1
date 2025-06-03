using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance;

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    public int WinningScore = 3;

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

        UIManager.Instance.UpdateScore(playerId, playerScores[playerId]);

        if (playerScores[playerId] >= WinningScore)
        {
            Debug.Log($"?? P{playerId} 獲勝！");
            // 可以在這裡觸發遊戲結束，暫時先不處理 UI 結束畫面
        }
    }

    public int GetScore(int playerId)
    {
        return playerScores.ContainsKey(playerId) ? playerScores[playerId] : 0;
    }
}
