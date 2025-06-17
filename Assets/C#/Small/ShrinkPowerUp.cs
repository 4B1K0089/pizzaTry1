using UnityEngine;

public class ShrinkPowerUp : MonoBehaviour
{
    public float shrinkDuration = 5f;
    public float shrinkScale = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pizza"))
        {
            PlayerShrink shrink = other.GetComponent<PlayerShrink>();
            if (shrink != null)
            {
                shrink.StartShrink(shrinkScale, shrinkDuration);

                FindObjectOfType<ShrinkPowerUpManager>().PowerUpCollected();
                Destroy(gameObject); // «OÀI
            }
        }
    }
}
