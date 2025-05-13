using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager Instance;


    [Header("Countdown UI")]
    public GameObject countdownPanel;
    public TextMeshProUGUI countdownText;

    [Header("Shared Character Data")]
    public Sprite[] sharedCharacterSprites;

    [Header("Display Positions")]
    public Transform[] playerDisplayPositions;

    public List<PlayerSelector> activePlayers = new List<PlayerSelector>();
    private HashSet<int> lockedCharacters = new HashSet<int>();
    private int maxPlayers = 4;
    private bool countdownStarted = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        PlayerInputManager.instance.onPlayerJoined += OnPlayerJoined;
        countdownPanel.SetActive(false);
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int index = activePlayers.Count-0;

        if (index >= maxPlayers)
        {
            Destroy(playerInput.gameObject);
            return;
        }

        if (index < playerDisplayPositions.Length)
        {
            playerInput.transform.position = playerDisplayPositions[index].position;
        }

        playerInput.transform.SetParent(this.transform);
        PlayerSelector selector = playerInput.GetComponent<PlayerSelector>();
        selector.Initialize(index, sharedCharacterSprites, playerDisplayPositions[index]);
        RegisterPlayer(selector);
    }

    public void RegisterPlayer(PlayerSelector player)
    {
        if (!activePlayers.Contains(player) && activePlayers.Count < maxPlayers)
        {
            activePlayers.Add(player);
        }
        Debug.Log($"目前玩家總數: {activePlayers.Count}");
    }

    public void ConfirmPlayerSelection(int playerIndex, int characterIndex)
    {
        if (lockedCharacters.Contains(characterIndex)) return;
        lockedCharacters.Add(characterIndex);
    }

    public bool IsCharacterTaken(int characterIndex)
    {
        return lockedCharacters.Contains(characterIndex);
    }

    public void UpdateReady()
    {
        foreach (var player in activePlayers)
        {
            if (!player.isReady)
                return;
        }

        if (!countdownStarted)
        {
            countdownStarted = true;
            ShowCountdownPanel();
        }
    }

    private void ShowCountdownPanel()
    {
        countdownPanel.SetActive(true);
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("GameScene");
    }
}
