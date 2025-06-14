using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public static PlayerScoreManager Instance;

    private Dictionary<int, int> playerScores = new Dictionary<int, int>();

    public int WinningScore = 3;

    [Header("Audio")]
    public AudioClip scoreClip; // ��i�o������
    public AudioSource audioSource; // ��i�@�� AudioSource�]��ĳ�P�@�Ӫ���W�^

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
        Debug.Log($"P{playerId} �o�� 1 ���A�{�b�O {playerScores[playerId]} ��");

        // ? ����o������
        if (scoreClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(scoreClip);
        }

        UIManager.Instance.UpdateScore(playerId, playerScores[playerId]);

        if (playerScores[playerId] >= WinningScore)
        {
            Debug.Log($"?? P{playerId} ��ӡI");
            GameOverUIManager.Instance.ShowGameOver(playerScores, playerId);
        }
    }

    public int GetScore(int playerId)
    {
        return playerScores.ContainsKey(playerId) ? playerScores[playerId] : 0;
    }
}
