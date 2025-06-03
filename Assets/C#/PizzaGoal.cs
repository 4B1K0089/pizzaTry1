using UnityEngine;

public class PizzaGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pizza"))
        {
            Debug.Log(other.name + " ���\�g�J�j���ġI");

            PlayerController controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                int playerId = controller.playerId;
                PlayerScoreManager.Instance.AddScore(playerId, 1);
            }
        }
    }
}
