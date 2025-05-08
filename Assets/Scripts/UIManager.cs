using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviourPunCallbacks
{
    [Header("Welcome UI")]
    [SerializeField] private GameObject wlc_Panel;
    [SerializeField] private Button playBtn;

    [Header("Lobby")]
    public TMP_InputField createRoom_Input;
    public TMP_InputField joinRoom_Input;
    public GameObject noRoomError;
    public Button createRoomBtn;
    public Button JoinRoomBtn;

    [Header("Scripts Ref")]
    [SerializeField] private LobbyManager lobbyManager;

    #region MonoBehaviou
    public override void OnEnable()
    {
        base.OnEnable();
        playBtn.onClick.AddListener(OnPlayBtn);
        createRoomBtn.onClick.AddListener(OnCreateRoom);
        JoinRoomBtn.onClick.AddListener(OnJoinRoom);
    }
    public override void OnDisable()
    {
        base.OnDisable();
        playBtn.onClick.RemoveAllListeners();
    }
    private void Awake()
    {
        wlc_Panel.SetActive(true);
    }
    #endregion

    #region Buttons Actions
    private void OnCreateRoom() 
    {
        lobbyManager.CreateRoom(createRoom_Input.text);
    }

    private void OnJoinRoom() 
    {
        lobbyManager.JoinRoom(joinRoom_Input.text);
    }
    private void OnPlayBtn() 
    {
        wlc_Panel.SetActive(false);
    }
    #endregion

    public IEnumerator DeactivateErrorMessage()
    {
        noRoomError.SetActive(true);
        yield return new WaitForSeconds(2f);
        noRoomError.SetActive(false);
    }
    //public override void OnLeftRoom()
    //{
    //    SceneManager.LoadScene(0);
    //}

    //public void LeaveRoom()
    //{
    //    PhotonNetwork.LeaveRoom();
    //}
}
