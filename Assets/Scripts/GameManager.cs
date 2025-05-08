using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefabVR;
    [SerializeField] private GameObject playerPrefabNormal;
    [SerializeField] private int mainMenuScene = 0;

    private GameObject playerVR;
    private GameObject normalPlayer;
    private const byte KickAllPlayersEventCode = 1;
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetKeyDown(KeyCode.L) && PhotonNetwork.IsMasterClient)
        {
            LeaveRoomAndKickOthers();
        }
    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerVR = PhotonNetwork.Instantiate(playerPrefabVR.name, new Vector3(0, 1, 0), Quaternion.identity);
        }
        else
        {
            normalPlayer = PhotonNetwork.Instantiate(playerPrefabNormal.name, new Vector3(10, 1, 10), Quaternion.identity);
        }
    }

    private void LeaveRoomAndKickOthers()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.RaiseEvent(
                KickAllPlayersEventCode,
                null,
                new RaiseEventOptions { Receivers = ReceiverGroup.Others },
                new SendOptions { Reliability = true }
            );
        }
        PhotonNetwork.LeaveRoom();
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == KickAllPlayersEventCode)
        {
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(mainMenuScene);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LeaveRoom();
            SceneManager.LoadScene(mainMenuScene);
        }
    }
}
