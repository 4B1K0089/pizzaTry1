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

    public PlayerColorSetting[] playerColors; // �b Inspector ���]�w�C�Ӫ��a���C��
    public GameObject[] pizzaModels; // �u�t���ҫ��A���t PlayerInput
    public Transform[] spawnPoints;

    private List<int> availableModelIndices = new List<int>();
    private int playerCount = 0;

    private void Start()
    {
        // ��l�ƥi�μҫ����ެ� 0 ~ pizzaModels.Length-1
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
        Debug.Log($"�[�J�����a�˸m: {playerInput.devices[0].displayName}");

        if (availableModelIndices.Count == 0 || playerCount >= spawnPoints.Length)
        {
            Debug.LogWarning("�w�L�i�μҫ��ΥX���I�I");
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

            // �q�]�w�}�C��������C��
            Color colorToUse = Color.white;
            foreach (var setting in playerColors)
            {
                if (setting.playerId == controller.playerId)
                {
                    colorToUse = setting.color;
                    break;
                }
            }

            // �N�C��s�� controller�A�����i�H��
            controller.lineColor = colorToUse;

            // �p�G�� LineRenderer�A����]�w�C��
            if (controller.lineRenderer != null)
            {
                controller.lineRenderer.startColor = colorToUse;
                controller.lineRenderer.endColor = colorToUse;
            }

            Debug.Log($"�]�w P{controller.playerId} �C�⬰ {colorToUse}");
        }


        playerCount++;
    }

}
