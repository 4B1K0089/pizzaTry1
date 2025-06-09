using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerJoinManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerColorSetting
    {
        public int playerId;
        public Color color;
    }

    public PlayerColorSetting[] playerColors; // 在 Inspector 中設定每個玩家的顏色
    public GameObject[] pizzaModels; // 只含有模型，不含 PlayerInput
    public Transform[] spawnPoints;

    private List<int> availableModelIndices = new List<int>();
    private int playerCount = 0;

    private void Start()
    {
        // 初始化可用模型索引為 0 ~ pizzaModels.Length-1
        for (int i = 0; i < pizzaModels.Length; i++)
        {
            availableModelIndices.Add(i);
        }
    }

    private void OnEnable()
    {
        if (PlayerInputManager.instance != null)
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        if (PlayerInputManager.instance != null)
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log($"加入的玩家裝置: {playerInput.devices[0].displayName}");

        if (availableModelIndices.Count == 0 || playerCount >= spawnPoints.Length)
        {
            Debug.LogWarning("已無可用模型或出生點！");
            Destroy(playerInput.gameObject);
            return;
        }

        int randomIndex = Random.Range(0, availableModelIndices.Count);
        int selectedModelIndex = availableModelIndices[randomIndex];
        availableModelIndices.RemoveAt(randomIndex);

        Rigidbody rigidbody = playerInput.GetComponent<Rigidbody>();
        rigidbody.position = spawnPoints[playerCount].position;
        rigidbody.rotation = spawnPoints[playerCount].rotation;

        GameObject model = Instantiate(pizzaModels[selectedModelIndex], playerInput.transform);
        model.transform.localPosition = Vector3.zero;
        model.SetActive(true);

        PlayerController controller = playerInput.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.playerId = playerCount + 1;

            // 從設定陣列中找對應顏色
            Color colorToUse = Color.white;
            foreach (var setting in playerColors)
            {
                if (setting.playerId == controller.playerId)
                {
                    colorToUse = setting.color;
                    break;
                }
            }

            // 將顏色存給 controller，讓它可以用
            controller.lineColor = colorToUse;

            // 如果有 LineRenderer，當場設定顏色
            if (controller.lineRenderer != null)
            {
                controller.lineRenderer.startColor = colorToUse;
                controller.lineRenderer.endColor = colorToUse;
            }

            Debug.Log($"設定 P{controller.playerId} 顏色為 {colorToUse}");
        }


        playerCount++;
    }

}
