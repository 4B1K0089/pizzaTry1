using UnityEngine;

public class PizzaUIFollow : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public Vector3 worldOffset;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (target == null || cam == null || rectTransform == null) return;

        Vector3 worldPos = target.position + worldOffset;
        Vector3 screenPos = cam.GetComponent<Camera>().WorldToScreenPoint(worldPos);

        Vector2 anchoredPos;
        RectTransform parentRect = rectTransform.parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect, screenPos, cam.GetComponent<Camera>(), out anchoredPos))
        {
            rectTransform.anchoredPosition = anchoredPos;
        }
        Debug.Log($"Follow Pos: {target.position}, UI Screen Pos: {screenPos}");

    }

}
