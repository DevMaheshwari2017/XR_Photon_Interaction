using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefabVR;
    [SerializeField] private GameObject playerPrefabNormal;

    private GameObject playerVR;
    private GameObject normalPlayer;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)) 
        {
            Cursor.lockState = CursorLockMode.None;
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
}
