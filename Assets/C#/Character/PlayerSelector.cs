using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerSelector : MonoBehaviour
{
    public int playerIndex;
    private Sprite[] characterSprites;

    private int currentIndex = 0;
    public bool isReady = false;
    private PlayerInput playerInput;
    private bool inputLocked = false;

    private Transform displayImageObject;
    //private Transform playerDisplayPositions[index];

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.currentActionMap.FindAction("Move").performed += OnMove;
        playerInput.currentActionMap.FindAction("Confirm").performed += OnConfirm;
    }


    // 外部呼叫初始化
    public void Initialize(int index, Sprite[] sprites, Transform imageObject)
    {
        playerIndex = index;
        characterSprites = sprites;
        displayImageObject = imageObject;
        //displayImageObject.SetParent(playerDisplayPositions[index].Transform);
        UpdateImageDisplay();
    }

    void OnMove(InputAction.CallbackContext context)
    {
        if (inputLocked || !isReady) return;

        Vector2 move = context.ReadValue<Vector2>();

        if (move.x > 0.5f)
        {
            currentIndex = (currentIndex + 1) % characterSprites.Length;
            UpdateImageDisplay();
            StartCoroutine(UnlockInputDelay());
        }
        else if (move.x < -0.5f)
        {
            currentIndex = (currentIndex - 1 + characterSprites.Length) % characterSprites.Length;
            UpdateImageDisplay();
            StartCoroutine(UnlockInputDelay());
        }
    }

    void OnConfirm(InputAction.CallbackContext context)
    {
        if (!isReady)
        {
            isReady = true;
            MultiplayerManager.Instance.ConfirmPlayerSelection(playerIndex, currentIndex);
            MultiplayerManager.Instance.UpdateReady();
            Debug.Log($"Player {playerIndex} confirmed image {currentIndex}");
        }
    }

    // 更新圖片顯示
    public void UpdateImageDisplay()
    {
        if (displayImageObject != null && characterSprites != null)
        {
            SpriteRenderer renderer = displayImageObject.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sprite = characterSprites[currentIndex];
            }
        }
    }

    IEnumerator UnlockInputDelay()
    {
        inputLocked = true;
        yield return new WaitForSeconds(0.2f);
        inputLocked = false;
    }
}
