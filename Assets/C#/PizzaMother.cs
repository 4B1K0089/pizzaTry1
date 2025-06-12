using UnityEngine;

public class PizzaMother : MonoBehaviour
{
    public float impactForce = 5f;        // 撞小披薩的移動力
    public float bounceForce = 5f;        // 撞牆時的反彈力
    public float moveDuration = 0.5f;     // 反彈持續時間

    private Rigidbody rb;
    private Vector3 moveDirection = Vector3.zero;
    private float moveTimer = 0f;
    private bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            moveTimer += Time.fixedDeltaTime;
            rb.MovePosition(rb.position + moveDirection * Time.fixedDeltaTime);

            if (moveTimer >= moveDuration)
            {
                isMoving = false;
                moveTimer = 0f;
                moveDirection = Vector3.zero;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pizza") && !isMoving)
        {
            Vector3 impactDir = (transform.position - collision.contacts[0].point).normalized;
            impactDir.y = 0;
            moveDirection = impactDir * impactForce;
            isMoving = true;
            moveTimer = 0f;
            Debug.Log("撞到小披薩，產生位移！");
        }

        if (collision.gameObject.CompareTag("Wall") && !isMoving)
        {
            Vector3 normal = collision.contacts[0].normal;
            moveDirection = Vector3.Reflect(rb.velocity.normalized, normal) * bounceForce;
            isMoving = true;
            moveTimer = 0f;
            Debug.Log("撞牆反彈！");
        }
    }
}
