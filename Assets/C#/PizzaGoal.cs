using UnityEngine;

public class PizzaGoal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pizza"))
        {
            Debug.Log(other.name + " ���\�g�J�j���ġI");
            // �o�̥i�HĲ�o�o���B�ʵe�ιC������
        }
    }
}
