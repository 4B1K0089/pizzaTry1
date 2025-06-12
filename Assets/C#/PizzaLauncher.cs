using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PizzaLauncher : MonoBehaviour
{
    public float maxPower = 20f;
    public float minPower = 5f;
    public float bounceForce = 1.5f;
    public float boundaryLimit = 4f;

    [SerializeField]
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction fireAction;
    private PlayerController controller;

    [SerializeField]
    private float _bounceDelay;

    [SerializeField]
    private float _minVelocityLength = 5;
    private Vector3 _lastVelocity = Vector3.right;

    [SerializeField]
    private float _minReflectAngle = 40;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.freezeRotation = true;

        playerInput = GetComponent<PlayerInput>();
        controller = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            fireAction = playerInput.actions["Select"]; // A鍵
            fireAction.performed += OnFirePerformed;
        }
    }

    private void OnDisable()
    {
        if (fireAction != null)
        {
            fireAction.performed -= OnFirePerformed;
        }
    }

    private void OnFirePerformed(InputAction.CallbackContext context)
    {
        if (controller != null && controller.IsCharging)
        {
            float lineLength = controller.GetLineLength();
            float normalizedCharge = Mathf.InverseLerp(0.1f, 2.5f, lineLength); // 可根據實際距離微調
            StartCoroutine(Launch(controller.LaunchDirection, normalizedCharge));
        }
    }

    private IEnumerator Launch(Vector3 direction, float charge)
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 新的力道與Drag設定
        float power = Mathf.Lerp(minPower, maxPower, charge); // 根據LineRenderer長度變化
        rb.AddForce(direction * power, ForceMode.Impulse);

        float maxDrag = 5f;
        float minDrag = 0.2f;
        rb.drag = Mathf.Lerp(maxDrag, minDrag, charge); // 長度越長，drag越小

        Debug.Log($"發射！方向: {direction}, 力度: {power}, Drag: {rb.drag}");
        yield return null;
    }

    private void FixedUpdate()
    {
        RecordLastVelocity();
    }

    private void RecordLastVelocity()
    {
        if (rb.velocity.magnitude < _minVelocityLength)
        {
            if (rb.velocity.normalized == Vector3.zero) return;
            _lastVelocity = rb.velocity.normalized * _minVelocityLength;
        }
        else
        {
            _lastVelocity = rb.velocity;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            CollideWithWall(collision);
        }
        else if (collision.gameObject.CompareTag("Pizza"))
        {
            CollideWithPizza(collision);
        }
    }

    private void CollideWithWall(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        Debug.Log("撞牆！");
        ReflectAndBounce(normal);
    }

    private void CollideWithPizza(Collision collision)
    {
        Vector3 normal = collision.contacts[0].normal;
        Debug.Log("撞披薩！");
        ReflectAndBounce(normal);
    }

    private void ReflectAndBounce(Vector3 normal)
    {
        float angle = Vector3.Angle(_lastVelocity, normal);
        Vector3 reflectVector = Vector3.Reflect(_lastVelocity, normal);
        if (angle < _minReflectAngle)
        {
            reflectVector = Vector3.RotateTowards(normal * reflectVector.magnitude, reflectVector, (90 - _minReflectAngle) * Mathf.Deg2Rad, 0f);
        }
        rb.velocity = Vector3.zero;
        rb.AddForce(reflectVector * bounceForce, ForceMode.Impulse);
    }

}
