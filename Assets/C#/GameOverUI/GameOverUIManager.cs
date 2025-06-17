using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverUIManager : MonoBehaviour
{
    public static GameOverUIManager Instance;

    [Header("音效設定")]
    public AudioSource gameBGMSource;
    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioClip resultIntroSFX;
    public AudioClip resultLoopBGM;
    public AudioClip navigateClip;
    public AudioClip confirmClip;

    [Header("UI 元素")]
    public GameObject gameOverPanel;
    public TMP_Text resultsText;
    public Image[] winImages;
    public Button restartButton;
    public Button quitButton;
    private Button[] buttons;
    private int currentIndex = 0;

    private float holdTime = 0f;
    public float requiredHoldDuration = 1.5f; // 長按 1.5 秒
    private bool isHolding = false;


    [Header("按鈕顏色")]
    public Color highlightColor = Color.yellow;
    public Color normalColor = Color.white;

    private PlayerInputActions inputActions;
    private bool inputEnabled = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        inputActions = new PlayerInputActions();

        // 加上這行讓上下選擇有效
        inputActions.StartUI.Navigate.performed += ctx => Navigate(ctx.ReadValue<Vector2>());

        inputActions.StartUI.Submit.started += ctx => StartHold();
        inputActions.StartUI.Submit.canceled += ctx => CancelHold();
    }


    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void ShowGameOver(Dictionary<int, int> playerScores, int winnerId)
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        ShowSortedScores(playerScores);
        ShowWinImage(winnerId);
        inputEnabled = true;

        buttons = new Button[] { restartButton, quitButton };
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
        UpdateButtonHighlight();

        if (gameBGMSource != null && gameBGMSource.isPlaying)
            gameBGMSource.Pause();

        if (resultIntroSFX != null && bgmSource != null)
            bgmSource.PlayOneShot(resultIntroSFX);

        if (resultLoopBGM != null && bgmSource != null)
            StartCoroutine(PlayResultBGMWithDelay(resultIntroSFX.length));
    }

    private IEnumerator PlayResultBGMWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        bgmSource.clip = resultLoopBGM;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    void ShowSortedScores(Dictionary<int, int> playerScores)
    {
        List<KeyValuePair<int, int>> sorted = new List<KeyValuePair<int, int>>(playerScores);
        sorted.Sort((a, b) => b.Value.CompareTo(a.Value));

        string result = "Score\n";
        foreach (var entry in sorted)
            result += $"{entry.Key}P -------- {entry.Value} \n";

        resultsText.text = result;
    }

    void ShowWinImage(int winnerId)
    {
        for (int i = 0; i < winImages.Length; i++)
            winImages[i].gameObject.SetActive(i == winnerId - 1);
    }

    void Navigate(Vector2 direction)
    {
        if (!inputEnabled || buttons == null || buttons.Length == 0) return;

        int prevIndex = currentIndex;

        if (direction.y > 0.1f)
            currentIndex = Mathf.Max(currentIndex - 1, 0);
        else if (direction.y < -0.1f)
            currentIndex = Mathf.Min(currentIndex + 1, buttons.Length - 1);

        if (prevIndex != currentIndex && sfxSource != null && navigateClip != null)
            sfxSource.PlayOneShot(navigateClip);

        UpdateButtonHighlight();
    }

    void SelectButton()
    {
        if (!inputEnabled || buttons == null) return;

        if (sfxSource != null && confirmClip != null)
            sfxSource.PlayOneShot(confirmClip);

        buttons[currentIndex].onClick.Invoke();
    }

    void StartHold()
    {
        if (!inputEnabled) return;
        isHolding = true;
        holdTime = 0f;
        StartCoroutine(HoldSubmitCoroutine());
    }

    void CancelHold()
    {
        isHolding = false;
    }

    IEnumerator HoldSubmitCoroutine()
    {
        while (isHolding)
        {
            holdTime += Time.unscaledDeltaTime;
            if (holdTime >= requiredHoldDuration)
            {
                SelectButton();
                isHolding = false;
            }
            yield return null;
        }
    }


    void UpdateButtonHighlight()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock colors = buttons[i].colors;
            colors.normalColor = (i == currentIndex) ? highlightColor : normalColor;
            buttons[i].colors = colors;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
