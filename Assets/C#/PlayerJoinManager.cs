using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class PlayerJoinManager : MonoBehaviour
{
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

        // �q�Ѿl�� index ���H����@��
        int randomIndex = Random.Range(0, availableModelIndices.Count);
        int selectedModelIndex = availableModelIndices[randomIndex];

        // �q�C�������o�� index�A�קK���ƨϥ�
        availableModelIndices.RemoveAt(randomIndex);

        // ���ʪ��a��X���I
        playerInput.transform.position = spawnPoints[playerCount].position;
        playerInput.transform.rotation = spawnPoints[playerCount].rotation;

        // ��ҤƼҫ��A�]�� player ���l����
        GameObject model = Instantiate(pizzaModels[selectedModelIndex], playerInput.transform);
        model.transform.localPosition = Vector3.zero;
        model.SetActive(true); // �[�o��ӫO�Ҽҫ������


        playerCount++;
    }
}
