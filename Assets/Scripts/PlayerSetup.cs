using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayerSetup : MonoBehaviour, IInRoomCallbacks
{
    [SerializeField] private TextMeshProUGUI playerNameText;

    public PhotonView photonView;
    public static PlayerSetup Instance; 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        photonView = GetComponent<PhotonView>();
        if (photonView == null) 
        {
            photonView = GetComponentInChildren<PhotonView>();
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            string playerName = $"player_{PhotonNetwork.LocalPlayer.ActorNumber}";

            Hashtable customProps = new Hashtable
            {
                { PlayerPropertyKeys.PlayerName, playerName }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);

            playerNameText.text = playerName;
            Debug.Log("Player name is : " + playerName);
        }
        else
        {
            if (photonView.Owner.CustomProperties.TryGetValue(PlayerPropertyKeys.PlayerName, out object name))
            {
                playerNameText.text = name.ToString();
            }
        }
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer == photonView.Owner && changedProps.ContainsKey(PlayerPropertyKeys.PlayerName))
        {
            playerNameText.text = changedProps[PlayerPropertyKeys.PlayerName].ToString();
        }
    }

    // Unused IInRoomCallbacks (but must be implemented)
    public void OnPlayerEnteredRoom(Player newPlayer) { }
    public void OnPlayerLeftRoom(Player otherPlayer) { }
    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) { }
    public void OnMasterClientSwitched(Player newMasterClient) { }
}

public static class PlayerPropertyKeys
{
    public const string PlayerName = "PlayerName";
}
