using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Microsoft.MixedReality.Toolkit.UI;
using WiapMR.PlayerScripts;
using WiapMR.GUI;
using WiapMR.GameScripts;

namespace WiapMR.PUN
{
    /// <summary>
    /// Handles everything around the Photon Network like Connection, Ownership, ...
    /// </summary>
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

        /// <summary>
        /// Spawns player head objects on room join
        /// </summary>
        public override void OnJoinedRoom()
        {
            Debug.Log("Joined Room");
            GetComponent<PlayerManager>().Initialize();
            GetComponent<PlayerManager>().HeadHelper.GetComponent<SyncPos>()
                .photonView.RPC("SpawnHead", RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber);
            GetComponent<PlayerManager>().BoardHelper.GetComponent<SyncPos>()
                .photonView.RPC("SpawnHead", RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber);
            GameObject.FindObjectOfType<StartPlate>().OnJoinRoom();
            // GameObject.FindObjectOfType<StartPlate>().ChessClick();
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Is not master client");
            }
            else
            {
                Debug.Log("Is master client");
            }
        }

        /// <summary>
        /// Triggers the RPC functions normally called on join to other clients. This way they are in a correct state.
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            GetComponent<PlayerManager>().HeadHelper.GetComponent<SyncPos>()
                .photonView.RPC("SpawnHead", newPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
            GetComponent<PlayerManager>().BoardHelper.GetComponent<SyncPos>()
                .photonView.RPC("SpawnHead", newPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
            GameObject.FindObjectOfType<StartPlate>().EnableButtons();
        }
        public override void OnLeftRoom()
        {
            Debug.Log("Left Room :(");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.Log("Join Room Failed returnCode: " + returnCode + " message: " + message);
        }

        /// <summary>
        /// Handles Ownership requests for photonviews this client owns.
        /// </summary>
        /// <param name="targetView"></param>
        /// <param name="requestingPlayer"></param>
        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            Debug.Log("Ownership has been requested for: " + targetView.ViewID + " by: " + requestingPlayer.NickName);
            if (targetView.IsMine)
            {
                // Debug.Log("I own this object");
                //only let the OwnershipRequest go through when the object is not being grabbed
                if (targetView.gameObject.tag == "GamePiece")
                {
                    if (!targetView.gameObject.GetComponent<PlaceableObject>().IsGrabbing)
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

        /// <summary>
        /// Handles enabling/disabling handling of gamePieces when ownership is lost or gained.
        /// </summary>
        /// <param name="targetView"></param>
        /// <param name="previousOwner"></param>
        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            if (previousOwner == PhotonNetwork.LocalPlayer && targetView.gameObject.tag == "GamePiece")
            {
                //stop the GamePiece from being moved by local player if they are no longer the owner
                targetView.gameObject.GetComponent<ObjectManipulator>().enabled = false;
                Debug.Log("Disabled ObjectManipulator on " + targetView.gameObject + " by " + previousOwner);
                // targetView.gameObject.GetComponent<ObjectManipulator>().ManipulationType = 0;
            }
            if (targetView.IsMine && targetView.gameObject.tag == "GamePiece")
            {
                targetView.gameObject.GetComponent<ObjectManipulator>().enabled = true;
                Debug.Log("Enabled GamePiece Manipulator");
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