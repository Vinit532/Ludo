using System.Collections;
using Photon.Pun;
using UnityEngine;

public class TurnManager : MonoBehaviourPun
{
    public static TurnManager Instance { get; private set; }

    private int currentPlayerIndex = 0;
    private PlayerManager[] players;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        players = FindObjectsOfType<PlayerManager>();
        StartNextTurn();
    }

    public bool IsPlayerTurn()
    {
        if (photonView.IsMine && players != null && currentPlayerIndex >= 0 && currentPlayerIndex < players.Length)
        {
            return players[currentPlayerIndex].photonView.IsMine;
        }

        return false;
    }

    public void StartNextTurn()
    {
        StartCoroutine(NextTurnDelay());
    }

    private IEnumerator NextTurnDelay()
    {
        yield return new WaitForSeconds(1f); // Add a small delay to prevent instant re-rolls

        // Go to the next player's turn
        currentPlayerIndex++;
        if (currentPlayerIndex >= players.Length)
        {
            currentPlayerIndex = 0;
        }

        // Call RollDice for the current player
        if (IsPlayerTurn())
        {
            players[currentPlayerIndex].RollDice();
        }
    }
}
