using UnityEngine;
using System.Collections;

public class ShrinkPowerUpManager : MonoBehaviour
{
    public GameObject powerUpPrefab;
    public Vector3 minBounds; // 設定可生成的最小座標（牆內）
    public Vector3 maxBounds; // 設定可生成的最大座標（牆內）

    private GameObject currentPowerUp;

    void Start()
    {
        StartCoroutine(SpawnPowerUpAfterDelay(5f));
    }

    IEnumerator SpawnPowerUpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnPowerUp();
    }

    void SpawnPowerUp()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(minBounds.x, maxBounds.x),
            Random.Range(minBounds.y, maxBounds.y),
            Random.Range(minBounds.z, maxBounds.z)
        );

        currentPowerUp = Instantiate(powerUpPrefab, spawnPos, Quaternion.identity);
        currentPowerUp.SetActive(true);
    }

    public void PowerUpCollected()
    {
        if (currentPowerUp != null)
        {
            Destroy(currentPowerUp);
            currentPowerUp = null;
        }

        StartCoroutine(SpawnPowerUpAfterDelay(8f));
    }
}
