using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    [Header("Predefined Score UI Elements")]
    [SerializeField] private List<TextMeshProUGUI> playerScoreUIs;

    private Dictionary<string, int> playerScores = new();
    private Dictionary<string, TextMeshProUGUI> scoreTexts = new();
    private int currentUIIndex = 0;

    public static ScoreManager Instance;

    #region MonoBeahviour Function
    private void Awake()
    {
        if (Instance == null) Instance = this;

        // Deactivate all UI entries initially
        foreach (var text in playerScoreUIs)
            text.gameObject.SetActive(false);
    }

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            TryAddPlayerEntry(player);
        }
    }
    #endregion

    #region Public Functions
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        TryAddPlayerEntry(newPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (changedProps.ContainsKey(PlayerPropertyKeys.PlayerName))
        {
            TryAddPlayerEntry(targetPlayer);
        }
    }
    public void AddScore(int amount, PhotonView view)
    {
        if (!view.IsMine) return;

        if (view.Owner.CustomProperties.TryGetValue(PlayerPropertyKeys.PlayerName, out object nameObj))
        {
            string playerName = nameObj as string;
            this.photonView.RPC(nameof(UpdateScoreRPC), RpcTarget.AllBuffered, playerName, amount);
        }
    }
    #endregion

    #region Private Functions
    private void TryAddPlayerEntry(Player player)
    {
        if (player.CustomProperties.TryGetValue(PlayerPropertyKeys.PlayerName, out object nameObj))
        {
            string playerName = nameObj as string;

            if (!playerScores.ContainsKey(playerName) && currentUIIndex < playerScoreUIs.Count)
            {
                playerScores[playerName] = 0;

                TextMeshProUGUI scoreText = playerScoreUIs[currentUIIndex];
                scoreText.text = $"{playerName}: 0";
                scoreText.gameObject.SetActive(true);

                scoreTexts[playerName] = scoreText;
                currentUIIndex++;
            }
        }
    }


    [PunRPC]
    private void UpdateScoreRPC(string playerName, int amount)
    {
        if (!playerScores.ContainsKey(playerName))
            playerScores[playerName] = 0;

        playerScores[playerName] += amount;

        if (scoreTexts.ContainsKey(playerName))
        {
            scoreTexts[playerName].text = $"{playerName}: {playerScores[playerName]}";
        }
    }
    #endregion

}
