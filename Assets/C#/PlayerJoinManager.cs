using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    public GameObject[] playerPrefabs; // ���P���ļҫ��� Prefab �}�C
    public Transform[] spawnPoints2P; // 2P �Ҧ����X���I
    public Transform[] spawnPoints4P; // 4P �Ҧ����X���I

    private int playerCount = 0; // ��e���a�ƶq

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;

        // ��ܥX���I
        Transform spawnPoint = (playerCount <= 2) ? spawnPoints2P[playerCount - 1] : spawnPoints4P[playerCount - 1];

        // �T�O���������ҫ��A�_�h�w�]���Ĥ@��
        GameObject selectedPrefab = playerPrefabs[playerCount - 1 % playerPrefabs.Length];

        // �ͦ����P�ҫ������a
        GameObject newPlayer = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
        playerInput.transform.SetParent(newPlayer.transform, false);

        Debug.Log($"���a {playerInput.playerIndex + 1} �[�J�A�ϥ� {selectedPrefab.name}�A��m�G{spawnPoint.position}");
    }
}
