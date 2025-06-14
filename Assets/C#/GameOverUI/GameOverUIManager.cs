using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class GameOverUIManager : MonoBehaviour
{

    public static GameOverUIManager Instance;

    [Header("音效設定")]
    public AudioSource gameBGMSource;     // 遊戲中播放的背景音樂（會暫停）
    public AudioSource bgmSource;         // 結算用 AudioSource
    public AudioClip resultIntroSFX;      // 開場提示音效
    public AudioClip resultLoopBGM;       // 結算背景音樂



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


        // ? 暫停遊戲背景音樂
        if (gameBGMSource != null && gameBGMSource.isPlaying)
            gameBGMSource.Pause();

        // ? 播放開場提示音效
        if (resultIntroSFX != null && bgmSource != null)
            bgmSource.PlayOneShot(resultIntroSFX);

        // ? 延遲播放結算背景音樂
        if (resultLoopBGM != null && bgmSource != null)
            StartCoroutine(PlayResultBGMWithDelay(resultIntroSFX.length));
    }

    private System.Collections.IEnumerator PlayResultBGMWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        bgmSource.clip = resultLoopBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }



    void ShowSortedScores(Dictionary<int, int> playerScores)
    {
        List<KeyValuePair<int, int>> sorted = new List<KeyValuePair<int, int>>(playerScores);
        sorted.Sort((a, b) => b.Value.CompareTo(a.Value)); // 降序排序

        string result = "Score\n";
        foreach (var entry in sorted)
        {
            result += $"{entry.Key}P: {entry.Value} \n";
        }

        resultsText.text = result;
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
