using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace WiapMR.PUN
{
    public class PUN_Controller : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
    {
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
            //PhotonNetwork.SendRate = 60; // 60 updates per second; default is 20
        }

        public override void OnConnectedToMaster()
        {
            //Always connect to the same room
            Debug.Log("Connected to Master");
            PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");
            GameObject.Find("ButtonHelper").GetComponent<ButtonHelper>().EnableButtons();
            //PhotonNetwork.Instantiate(photonUserPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.Log("Join Room Failed returnCode: " + returnCode + " message: " + message);
        }

        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            if (targetView.IsMine)
            {
                //only let the OwnershipRequest go through when the object is not being grabbed
                if (targetView.gameObject.tag == "Cube")
                {
                    if (!targetView.gameObject.GetComponent<CubeController>().isGrabbing)
                    {
                        targetView.TransferOwnership(requestingPlayer);
                        Debug.Log("Ownership Transfer Requested on " + targetView.gameObject + " by " + requestingPlayer);
                    }
                    else
                    {
                        Debug.Log("Ownership transfer denied: Object is being grabbed!");
                    }
                }
            }
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            if (previousOwner == PhotonNetwork.LocalPlayer && targetView.gameObject.tag == "Cube")
            {
                //stop the cube from being moved by local player if they are no longer the owner
                targetView.gameObject.GetComponent<ObjectManipulator>().enabled = false;
                Debug.Log("Disabled ObjectManipulator on " + targetView.gameObject + " by " + previousOwner);
                // targetView.gameObject.GetComponent<ObjectManipulator>().ManipulationType = 0;
            }
            if (targetView.IsMine && targetView.gameObject.tag == "Cube")
            {
                targetView.gameObject.GetComponent<ObjectManipulator>().enabled = true;
                Debug.Log("Enabled Cube Manipulator");
                // targetView.gameObject.GetComponent<ObjectManipulator>().ManipulationType = ManipulationHandFlags.OneHanded | ManipulationHandFlags.TwoHanded;
            }
            Debug.Log("Ownership Transfered from " + previousOwner + " to " + targetView.Owner);
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
            Debug.Log("Ownership Transfer Failed");
        }
    }
}