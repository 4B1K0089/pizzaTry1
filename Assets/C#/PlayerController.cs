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

    private Vector2 moveInput;
    private Gamepad assignedGamepad;
    public bool IsCharging { get; private set; } = false;

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
        }
    }

    private void Update()
    {
        if (assignedGamepad == null) return;

        moveInput = assignedGamepad.leftStick.ReadValue();

        if (moveInput.magnitude > 0.1f)
        {
            IsCharging = true;

            Vector3 inputDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            LaunchDirection = -inputDir; // 指向反方向（如彈射）

            ChargeAmount = Mathf.Clamp01(moveInput.magnitude); // 限制 0~1

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

}
