using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PizzaLauncher : MonoBehaviour
{
    public float maxPower = 20f;
    public float minPower = 5f;
    public float bounceForce = 1.5f;
    public float boundaryLimit = 4f;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction fireAction;
    private PlayerController controller;


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
            StartCoroutine(Launch(controller.LaunchDirection, controller.ChargeAmount));
        }
    }

    private IEnumerator Launch(Vector3 direction, float charge)
    {

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        float power = Mathf.Max(charge * maxPower, minPower);
        rb.AddForce(direction * power, ForceMode.Impulse);

        float maxDrag = 5f;
        float minDrag = 0.2f;
        rb.drag = Mathf.Lerp(maxDrag, minDrag, charge);

        Debug.Log($"發射！方向: {direction}, 力度: {power}, Drag: {rb.drag}");
        yield return null;
    }

    private void Update()
    {
        ClampPizzaPosition();
    }

    private void ClampPizzaPosition()
    {
        float x = Mathf.Clamp(transform.position.x, -boundaryLimit, boundaryLimit);
        float z = Mathf.Clamp(transform.position.z, -boundaryLimit, boundaryLimit);
        transform.position = new Vector3(x, transform.position.y, z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Wall"))
        //{
        //    Vector3 normal = collision.contacts[0].normal;
        //    rb.velocity = Vector3.Reflect(rb.velocity, normal) * bounceForce;
        //    Debug.Log("撞牆！");
        //}

    }
}
