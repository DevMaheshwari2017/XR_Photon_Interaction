using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem.XR;
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


            // Disable XRDeviceSimulator or controllers
            XRDeviceSimulator simulator = GetComponentInChildren<XRDeviceSimulator>();
            if (simulator != null) simulator.enabled = false;

            // Disable InputAction-based controllers
            foreach (var controller in GetComponentsInChildren<ActionBasedController>())
            {
                if(controller != null)
                controller.enabled = false;
            }

            foreach (var poseDriver in GetComponentsInChildren<TrackedPoseDriver>())
            {
                if(poseDriver != null)
                poseDriver.enabled = false;
            }
        }
    }
}
