using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text[] scoreTexts; // 0=P1, 1=P2, 2=P3, 3=P4
    public AudioSource bgmSource; // ���w�I�����֪� AudioSource
    public AudioClip bgmClip;     // ���w�n���񪺭���

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        // �T�O�o�Ӫ��󤣷|�]���������Q�P���]�p�G�A�����������ݨD�^
        // DontDestroyOnLoad(gameObject);

        // ����I�����֡]�p�G������^
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
