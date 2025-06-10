using UnityEngine;

public class PizzaUIFollow : MonoBehaviour
{
    public Transform target;       // �n���H�����Ĩ���
    public Transform cam;          // ��v�� transform
    public Vector3 worldOffset;    // �Z�����Ī������]�Ҧp�GVector3.up * 2�^

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        // �@�ɮy�� + offset �� �ù��y��
        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // �]�w UI �b�ù��W����m
        rectTransform.position = screenPos;
    }
}
