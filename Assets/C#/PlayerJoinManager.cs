using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerJoinManager : MonoBehaviour
{
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

        // 從剩餘的 index 中隨機選一個
        int randomIndex = Random.Range(0, availableModelIndices.Count);
        int selectedModelIndex = availableModelIndices[randomIndex];

        // 從列表中移除這個 index，避免重複使用
        availableModelIndices.RemoveAt(randomIndex);

        // 移動玩家到出生點        
        Rigidbody rigidbody = playerInput.GetComponent<Rigidbody>();
        rigidbody.position = spawnPoints[playerCount].position;
        rigidbody.rotation = spawnPoints[playerCount].rotation;        

        // 實例化模型，設為 player 的子物件
        GameObject model = Instantiate(pizzaModels[selectedModelIndex], playerInput.transform);
        model.transform.localPosition = Vector3.zero;
        model.SetActive(true); // 加這行來保證模型有顯示

        // 指定 playerId（1P, 2P...）
        PlayerController controller = playerInput.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.playerId = playerCount + 1; // 第 0 位是 P1（+1）
            Debug.Log($"設定玩家編號為 P{controller.playerId}");
        }
        playerCount++;
    }
}
