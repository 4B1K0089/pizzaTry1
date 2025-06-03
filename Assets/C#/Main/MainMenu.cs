using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;  // 確保有引用新 Input System

public class MainMenu : MonoBehaviour
{
    private PlayerInputActions inputActions;
    public Button startButton;
    public Button quitButton;
    private Button[] buttons;
    private int currentSelection = 0;

    [Header("StartUI Settings")]
    public Color highlightColor = Color.green; // ✅ 可在 Unity 修改
    public Color normalColor = Color.white;    // 普通按鈕顏色

    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.StartUI.Submit.performed += ctx => SelectButton();
        inputActions.StartUI.Navigate.performed += ctx => Navigate(ctx.ReadValue<Vector2>());
        inputActions.Enable();
    }

    void Start()
    {
        buttons = new Button[] { startButton, quitButton };
        UpdateButtonHighlight();

        startButton.onClick.AddListener(StartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    void Navigate(Vector2 direction)
    {
        if (direction.x > 0.1f) // 右移
        {
            currentSelection = Mathf.Min(currentSelection + 1, buttons.Length - 1);
        }
        else if (direction.x < -0.1f) // 左移
        {
            currentSelection = Mathf.Max(currentSelection - 1, 0);
        }

        UpdateButtonHighlight();
    }

    void OnDestroy()
    {
        inputActions.Disable();
    }

    void SelectButton()
    {
        buttons[currentSelection].onClick.Invoke();
    }

    void UpdateButtonHighlight()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            ColorBlock colors = buttons[i].colors;
            colors.normalColor = (i == currentSelection) ? highlightColor : normalColor;
            buttons[i].colors = colors;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
