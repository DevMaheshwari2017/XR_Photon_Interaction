using UnityEngine;
using Photon.Pun;
public class CameraManager : MonoBehaviour
{
   private PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine) 
        {
            Camera cam = GetComponentInChildren<Camera>();
            cam.enabled = false;
        }
    }

    /// <summary>
    /// As I am using XR interaction toolkit and can't chnage the package script, disabling any input on the other player from here
    /// Will call this script before any other script in Unity settings, to block all the inputs properly.
    /// </summary>
    private void Update()
    {
        if (!view.IsMine)
            return;
    }
    private void FixedUpdate()
    {
        if (!view.IsMine)
            return;
    }

    private void LateUpdate()
    {
        if (!view.IsMine)
            return;
    }
}
