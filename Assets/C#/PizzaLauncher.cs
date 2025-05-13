using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PizzaLauncher : MonoBehaviour
{
    [Header("Power Settings")]
    public float maxPower = 20f;
    public float minPower = 5f;

    [Header("Bounce & Boundary")]
    public float bounceForce = 1.5f;
    public float boundaryLimit = 4f;

    [Header("Aiming Visuals")]
    public float maxLineLength = 2f;
    public Transform lineStartTransform;
    public LineRenderer lineRenderer;

    private Rigidbody rb;
    private Vector2 stickInput;
    private bool isCharging = false;
    private Vector3 launchDirection;
    private float cachedCharge = 0f;

    private PlayerInput playerInput;
    private Gamepad assignedGamepad;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput.devices.Count > 0 && playerInput.devices[0] is Gamepad pad)
        {
            assignedGamepad = pad;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.freezeRotation = true;

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (assignedGamepad == null) return;

        stickInput = assignedGamepad.leftStick.ReadValue();

        if (stickInput.magnitude > 0.1f)
        {
            isCharging = true;

            Vector3 inputDirection = new Vector3(stickInput.x, 0, stickInput.y).normalized;
            transform.forward = -inputDirection;
            launchDirection = transform.forward;

            cachedCharge = stickInput.magnitude;

            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                float lineLength = Mathf.Lerp(0, maxLineLength, cachedCharge);
                Vector3 arrowStart = lineStartTransform.position;
                Vector3 arrowEnd = arrowStart + (-transform.forward) * lineLength;
                lineRenderer.SetPosition(0, arrowStart);
                lineRenderer.SetPosition(1, arrowEnd);
            }
        }

        if (isCharging && stickInput.magnitude <= 0.1f)
        {
            StartCoroutine(LaunchPizza());
            isCharging = false;

            if (lineRenderer != null)
                lineRenderer.enabled = false;
        }

        ClampPizzaPosition();
    }

    private IEnumerator LaunchPizza()
    {
        yield return null;

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        float launchPower = Mathf.Max(cachedCharge * maxPower, minPower);
        rb.AddForce(launchDirection * launchPower, ForceMode.Impulse);

        float maxDrag = 5f;
        float minDrag = 0.2f;
        rb.drag = Mathf.Lerp(maxDrag, minDrag, cachedCharge);

        Debug.Log($"發射！方向: {launchDirection}, 力度: {launchPower}, Drag: {rb.drag}");
    }

    private void ClampPizzaPosition()
    {
        float x = Mathf.Clamp(transform.position.x, -boundaryLimit, boundaryLimit);
        float z = Mathf.Clamp(transform.position.z, -boundaryLimit, boundaryLimit);
        transform.position = new Vector3(x, transform.position.y, z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            rb.velocity = Vector3.Reflect(rb.velocity, normal) * bounceForce;
        }
    }
}
