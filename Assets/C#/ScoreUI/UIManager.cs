using UnityEngine;
using TMPro;


public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text[] scoreTexts; // 0=P1, 1=P2, 2=P3, 3=P4


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdateScore(int playerId, int score)
    {
        if (playerId >= 1 && playerId <= scoreTexts.Length)
        {
            scoreTexts[playerId - 1].text = $"{playerId}P: {score}";
        }
    }
}
