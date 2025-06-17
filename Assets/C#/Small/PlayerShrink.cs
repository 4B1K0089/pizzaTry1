using UnityEngine;
using System.Collections;

public class PlayerShrink : MonoBehaviour
{
    private Vector3 originalScale;
    private Coroutine shrinkCoroutine;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void StartShrink(float scaleFactor, float duration)
    {
        if (shrinkCoroutine != null)
            StopCoroutine(shrinkCoroutine);

        shrinkCoroutine = StartCoroutine(ShrinkRoutine(scaleFactor, duration));
    }

    IEnumerator ShrinkRoutine(float scaleFactor, float duration)
    {
        transform.localScale = originalScale * scaleFactor;

        yield return new WaitForSeconds(duration);

        transform.localScale = originalScale;
        shrinkCoroutine = null;
    }
}
