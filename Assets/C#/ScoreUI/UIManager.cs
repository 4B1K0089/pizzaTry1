using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text[] scoreTexts; // 0=P1, 1=P2, 2=P3, 3=P4
    public AudioSource bgmSource; // 指定背景音樂的 AudioSource
    public AudioClip bgmClip;     // 指定要播放的音樂

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // 確保這個物件不會因切換場景被銷毀（如果你有場景切換需求）
        // DontDestroyOnLoad(gameObject);

        // 播放背景音樂（如果未播放）
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void UpdateScore(int playerId, int score)
    {
        if (playerId >= 1 && playerId <= scoreTexts.Length)
        {
            scoreTexts[playerId - 1].text = $"{playerId}P: {score}";
        }
    }
}
