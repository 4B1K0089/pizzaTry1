using UnityEngine;

public class PizzaUIFollow : MonoBehaviour
{
    public Transform target;       // 要跟隨的披薩角色
    public Transform cam;          // 攝影機 transform
    public Vector3 worldOffset;    // 距離披薩的偏移（例如：Vector3.up * 2）

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null) return;

        // 世界座標 + offset → 螢幕座標
        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        // 設定 UI 在螢幕上的位置
        rectTransform.position = screenPos;
    }
}
