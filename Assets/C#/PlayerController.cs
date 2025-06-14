using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int playerId; // 設定為 1, 2, 3, 4...
    public Vector3 LaunchDirection { get; private set; } = Vector3.forward;
    public float ChargeAmount { get; private set; } = 0f;

    [Header("Line Renderer")]
    public Transform lineStartTransform;
    public LineRenderer lineRenderer;
    public float maxLineLength = 2f;

    [Header("Line Renderer Settings")]
    public Color lineColor = Color.white;

    [Header("Audio")]
    public AudioSource audioSource;       // 指定 AudioSource（可共用）
    public AudioClip aimClip;             // 瞄準音效

    private Vector2 moveInput;
    private Gamepad assignedGamepad;
    public bool IsCharging { get; private set; } = false;

    private bool wasChargingLastFrame = false; // 用來偵測瞄準開始的變數

    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput.devices.Count > 0 && playerInput.devices[0] is Gamepad pad)
        {
            assignedGamepad = pad;
        }
    }

    private void Start()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;

            lineRenderer.startColor = lineColor;
            lineRenderer.endColor = lineColor;
        }
    }

    private void Update()
    {
        if (assignedGamepad == null) return;

        moveInput = assignedGamepad.leftStick.ReadValue();

        if (moveInput.magnitude > 0.1f)
        {
            IsCharging = true;

            // 當從沒按變成有按 → 播放瞄準音效
            if (!wasChargingLastFrame)
            {
                PlaySound(aimClip);
            }

            Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            LaunchDirection = -inputDir;

            ChargeAmount = Mathf.Clamp01(moveInput.magnitude);

            transform.forward = LaunchDirection;

            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                float lineLength = Mathf.Lerp(0, maxLineLength, ChargeAmount);
                Vector3 start = lineStartTransform.position;
                Vector3 end = start + (-transform.forward) * lineLength;
                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);
            }
        }
        else
        {
            if (lineRenderer != null)
                lineRenderer.enabled = false;

            IsCharging = false;
        }

        wasChargingLastFrame = IsCharging;
    }

    public float GetLineLength()
    {
        if (lineRenderer != null && lineRenderer.positionCount >= 2)
        {
            return Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));
        }
        return 0f;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
