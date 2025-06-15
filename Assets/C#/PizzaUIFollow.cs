using UnityEngine;
using UnityEngine.UI;

public class PizzaUIFollow : MonoBehaviour
{
    public Transform target; // ���a����
    public Camera cam;       // �D��v��
    public Vector3 worldOffset = new Vector3(0, 2, 0); // UI ���Y���h��


    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.WorldToScreenPoint(worldPos);

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            screenPos,
            cam,
            out anchoredPos
        );

        rectTransform.anchoredPosition = anchoredPos;
    }
}
