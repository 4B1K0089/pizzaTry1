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

    void Start()
    {
        canMove = false;
        //moveDirection = Vector3.forward * 1f; // 強制移動方向，測試用
    }


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
            //Debug.Log("進入 move 處理區");

            moveTimer += Time.deltaTime;
            float t = moveTimer / moveDuration;
            //Debug.Log("moveTimer: " + moveTimer + ", t: " + t);

            Vector3 targetPos = transform.position + moveDirection * Time.deltaTime;
            if (!Physics.Raycast(transform.position, moveDirection, 0.5f))
            {
                //Debug.Log("未偵測到前方有物體，移動中...");
                transform.position = targetPos;
            }
            else
            {
                //Debug.Log("偵測到牆壁，執行反彈");
                moveDirection = Vector3.Reflect(moveDirection, Vector3.right); // 建議這邊你可以改成使用撞擊法線
            }

            if (moveTimer >= moveDuration)
            {
                //Debug.Log("移動時間結束，恢復可移動狀態");
                canMove = true;
                moveTimer = 0f;
                moveDirection = Vector3.zero;
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("碰撞觸發：" + collision.gameObject.name + "，Tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Pizza") && canMove)
        {
            Vector3 impactDir = transform.position - collision.contacts[0].point;
            impactDir.y = 0;
            impactDir.Normalize();
            Debug.Log("撞到 Pizza！");

            moveDirection = impactDir * impactForce;
            canMove = false;
            moveTimer = 0f;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Vector3 normal = collision.contacts[0].normal;
            moveDirection = Vector3.Reflect(moveDirection.normalized, normal) * bounceForce;
            Debug.Log("撞牆反彈！");
        }
    }

}