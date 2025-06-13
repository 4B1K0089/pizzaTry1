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
    public GameObject[] uiPrefabs; // 拖入 PlayerName1 ~ 4 預製體
    public Transform canvasParent; // 指向 WorldCanvas（UI 要生成在哪）


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

        if (playerCount >= pizzaModels.Length || playerCount >= spawnPoints.Length)
        {
            Debug.LogWarning("已無可用模型或出生點！");
            Destroy(playerInput.gameObject);
            return;
        }

        int selectedModelIndex = playerCount; // 直接根據加入順序取模型

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

            // 設定顏色
            Color colorToUse = Color.white;
            foreach (var setting in playerColors)
            {
                if (setting.playerId == controller.playerId)
                {
                    colorToUse = setting.color;
                    break;
                }
            }

            controller.lineColor = colorToUse;

            if (controller.lineRenderer != null)
            {
                controller.lineRenderer.startColor = colorToUse;
                controller.lineRenderer.endColor = colorToUse;
            }

            Debug.Log($"設定 P{controller.playerId} 顏色為 {colorToUse}");
        }

        if (uiPrefabs != null && playerCount < uiPrefabs.Length)
        {
            GameObject uiInstance = Instantiate(uiPrefabs[playerCount], canvasParent);
            uiInstance.SetActive(true);

            PizzaUIFollow follow = uiInstance.GetComponent<PizzaUIFollow>();
            if (follow != null)
            {
                follow.target = playerInput.transform;
                follow.cam = Camera.main.transform;
                follow.worldOffset = new Vector3(0, 2f, 0);
            }
        }

        playerCount++;
    }


}
