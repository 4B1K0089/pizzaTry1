using UnityEngine;
using UnityEngine.UI;

public class PizzaUIFollow : MonoBehaviour
{
    public Transform target; // 玩家物件
    public Camera cam;       // 主攝影機
    public Vector3 worldOffset = new Vector3(0, 2, 0); // UI 離頭部多高


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
