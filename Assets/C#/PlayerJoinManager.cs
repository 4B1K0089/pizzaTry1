using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    public GameObject[] presetPlayers; // �w�]�����Ī���]Hierarchy ������n�^
    public Transform[] spawnPoints2P;
    public Transform[] spawnPoints4P;

    private HashSet<int> joinedDevices = new HashSet<int>();
    private int playerCount = 0;

    [SerializeField] private int maxPlayers = 4;

    private void OnEnable()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        }
    }

    private void OnDisable()
    {
        if (PlayerInputManager.instance != null)
        {
            PlayerInputManager.instance.onPlayerJoined -= OnPlayerJoined;
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        // �Y�˸m���j�w�h�ڵ�
        if (playerInput.devices.Count == 0)
        {
            Debug.LogWarning("�o�Ӫ��a�S���j�w�����J�˸m�I");
            Destroy(playerInput.gameObject);
            return;
        }

        int deviceId = playerInput.devices[0].deviceId;

        // ����O�_�w�[�J
        if (joinedDevices.Contains(deviceId))
        {
            Debug.LogWarning("�o�ӱ���w�g�[�J�L�F�I");
            Destroy(playerInput.gameObject);
            return;
        }

        if (playerCount >= maxPlayers)
        {
            Debug.LogWarning("�W�X�̤j���a�ơA�ڵ��[�J�I");
            Destroy(playerInput.gameObject);
            return;
        }

        // �X���I����޿�
        Transform[] spawnArray = (maxPlayers <= 2) ? spawnPoints2P : spawnPoints4P;

        if (playerCount >= spawnArray.Length)
        {
            Debug.LogError("Spawn point �ƶq�����I");
            Destroy(playerInput.gameObject);
            return;
        }

        if (presetPlayers == null || presetPlayers.Length == 0 || presetPlayers[playerCount] == null)
        {
            Debug.LogError("�w�]���Ī���]presetPlayers�^�|���]�w�I");
            Destroy(playerInput.gameObject);
            return;
        }

        // �Ыؤp���Ī��a����
        GameObject newPlayer = Instantiate(presetPlayers[playerCount], spawnArray[playerCount].position, spawnArray[playerCount].rotation);
        newPlayer.transform.SetParent(playerInput.transform, false); // �N PlayerInput �@���s�p���Ī�������

        // �T�O�����ƲK�[ PlayerInput
        PlayerInput newPlayerInput = newPlayer.GetComponent<PlayerInput>();
        if (newPlayerInput == null)
        {
            newPlayerInput = newPlayer.AddComponent<PlayerInput>();
        }

       // newPlayerInput.defaultControlScheme = "Gamepad";
        newPlayerInput.actions = playerInput.actions; // �~�ӭ즳����J�t�m

        // ��s���a���A
        joinedDevices.Add(deviceId);
        playerCount++;

        Debug.Log($"���a {playerInput.playerIndex + 1} �[�J�A��m�G{spawnArray[playerCount - 1].position}");
    }
}
