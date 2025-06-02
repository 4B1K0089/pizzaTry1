using UnityEngine;

public class PizzaGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pizza"))
        {
            Debug.Log(other.name + " 成功射入大披薩！");
            // 這裡可以觸發得分、動畫或遊戲結束

        }

    }
}
