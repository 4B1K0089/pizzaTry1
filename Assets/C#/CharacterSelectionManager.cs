using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionManager : MonoBehaviour
{
    public static CharacterSelectionManager Instance;

    // �s�x���ļҫ����w�s��
    public GameObject[] pizzaPrefabs;  // �C�Ӽҫ��������w�s��
    public Sprite[] pizzaSprites;  // �C�ө��Ī��Ϥ��]UI��ܥΡ^

    // UI ���s�w�s��
    public GameObject pizzaButtonPrefab;
    public Transform buttonContainer;

    // ��ܪ��a��ܪ����ĹϹ�
    public Image[] playerImages;  // ���a��ܪ����ĹϤ���ܡ]�̦h���4�Ӫ��a�^
    public Button startGameButton;

    // �x�s�C�Ӫ��a�����
    private Dictionary<int, int> playerSelections = new Dictionary<int, int>();

    private int totalPlayers = 2;  // �w�]2P�C���A�ھڻݭn�ʺA�]�m

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // �]�m���a�ƶq�A�åͦ������ƶq�����Ŀ�ܫ��s
    public void SetupPlayerSelectionUI(int playerCount)
    {
        totalPlayers = playerCount;

        // �M�z�{�������s
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // �Ыة��Ŀ�ܫ��s
        for (int i = 0; i < totalPlayers; i++)
        {
            GameObject pizzaButton = Instantiate(pizzaButtonPrefab, buttonContainer);
            pizzaButton.name = "PizzaButton" + (i + 1);
            pizzaButton.GetComponent<Button>().onClick.AddListener(() => OnPizzaButtonClicked(i));
            pizzaButton.GetComponentInChildren<Text>().text = "Player " + (i + 1);
        }

        // �]�m�}�l�C�����s�����i�I��
        startGameButton.interactable = false;
    }

    // ���a��ܩ��ī᪺�^��
    public void OnPizzaButtonClicked(int playerIndex)
    {
        if (!playerSelections.ContainsKey(playerIndex))
        {
            playerSelections.Add(playerIndex, -1);  // -1 ��ܩ|�����
        }

        // �H����ܤ@�ө��ļҫ��]�o�̰��]�O�H����ܡA�z�i�H�ھڻݭn�אּ��ʿ�ܡ^
        int pizzaIndex = Random.Range(0, pizzaSprites.Length); // �H�����
        playerSelections[playerIndex] = pizzaIndex;

        // ��sUI��ܪ��a��ܪ����ĹϹ�
        UpdatePizzaImage(playerIndex, pizzaSprites[pizzaIndex]);

        // �ˬd�O�_�Ҧ����a���w�g��ܤF����
        CheckAllPlayersSelected();
    }

    // ��s���a��ܪ����ĹϹ�
    private void UpdatePizzaImage(int playerIndex, Sprite selectedPizza)
    {
        if (playerIndex < playerImages.Length)
        {
            playerImages[playerIndex].sprite = selectedPizza;
        }
    }

    // �ˬd�O�_�Ҧ����a���w�g��ܤF����
    private void CheckAllPlayersSelected()
    {
        foreach (var selection in playerSelections)
        {
            if (selection.Value == -1)
            {
                return; // �Y�٦�����ܪ����a�A�h���Ұʶ}�l�C�����s
            }
        }

        // ��Ҧ����a��ܧ�����Ұʡu�}�l�C���v���s
        startGameButton.interactable = true;
    }

    // �}�l�C��
    public void StartGame()
    {
        // �o�̧ڭ̷|�ھڪ��a��ܪ����ިӥ[�����������ļҫ�
        for (int i = 0; i < totalPlayers; i++)
        {
            // �ھڪ��a��ܪ����ļҫ����ިӥ[�����������ļҫ�
            int pizzaIndex = playerSelections[i];

            // ��Ҥƪ��a����
            GameObject player = Instantiate(pizzaPrefabs[pizzaIndex], GetSpawnPosition(i), Quaternion.identity);
            player.name = "Player" + (i + 1);
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    // �ھڪ��a�s���]�m�X�ͦ�m�]�o�̰��]��Ӫ��a�M�|�Ӫ��a�����P���_�l��m�^
    private Vector3 GetSpawnPosition(int playerIndex)
    {
        if (totalPlayers == 2)
        {
            return playerIndex == 0 ? new Vector3(-5, 0, 0) : new Vector3(5, 0, 0);
        }
        else
        {
            // �|�H�C���ɳ]�m���P���_�l��m
            switch (playerIndex)
            {
                case 0: return new Vector3(-5, 0, -5);
                case 1: return new Vector3(5, 0, -5);
                case 2: return new Vector3(-5, 0, 5);
                case 3: return new Vector3(5, 0, 5);
                default: return Vector3.zero;
            }
        }
    }
}
