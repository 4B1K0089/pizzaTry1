using UnityEngine;
using System.Collections;

public class PizzaMother : MonoBehaviour
{
    public float rotationSpeed = 30f;     // 自轉速度
    public float impactForce = 5f;        // 撞擊時位移距離
    public float bounceForce = 5f;        // 撞牆時反彈強度
    public Transform detectionCube;       // 外部偵測 Cube
    public float moveDuration = 0.5f;     // 撞擊移動持續時間

    private bool canMove = true;
    private Vector3 moveDirection = Vector3.zero;
    private float moveTimer = 0f;

    void Update()
    {
        // 自轉
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // 偵測 Cube 同步位置與旋轉
        detectionCube.position = transform.position;
        detectionCube.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        // 平滑移動邏輯（模擬 impact 力）
        if (!canMove)
        {
            moveTimer += Time.deltaTime;
            float t = moveTimer / moveDuration;

            Vector3 targetPos = transform.position + moveDirection * Time.deltaTime;
            if (!Physics.Raycast(transform.position, moveDirection, 0.5f))
            {
                transform.position = targetPos;
            }
            else
            {
                // 撞牆，反彈方向
                moveDirection = Vector3.Reflect(moveDirection, Vector3.right); // 或依實際法線調整
            }

            if (moveTimer >= moveDuration)
            {
                canMove = true;
                moveTimer = 0f;
                moveDirection = Vector3.zero;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pizza") && canMove)
        {
            Vector3 impactDir = transform.position - collision.contacts[0].point;
            impactDir.y = 0;
            impactDir.Normalize();

            moveDirection = impactDir * impactForce;
            canMove = false;
            moveTimer = 0f;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            moveDirection = Vector3.Reflect(moveDirection.normalized, normal) * bounceForce;
        }
    }
}
