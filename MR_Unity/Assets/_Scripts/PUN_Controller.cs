using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Photon;

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
            GetComponent<PlayerManager>().Initialize();
            GetComponent<PlayerManager>().HeadHelper.GetComponent<SyncPos>()
                .photonView.RPC("SpawnHead", RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber);
            GetComponent<PlayerManager>().BoardHelper.GetComponent<SyncPos>()
                .photonView.RPC("SpawnHead", RpcTarget.Others, PhotonNetwork.LocalPlayer.ActorNumber);
            // this.photonView.RPC("MasterSend", RpcTarget.MasterClient);
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Is not master client");
                gameObject.GetComponent<GameImporter>().DoStuff();
            }
            else
            {
                // gameObject.GetComponent<GameImporter>().DoStuff();
                Debug.Log("Is master client, not spawning board");
                // gameObject.GetComponent<GameImporter>().CheckForPlayers(gameObject.GetComponent<GameImporter>().GameRoot);
            }

            // GameObject.Find("ButtonHelper").GetComponent<ButtonHelper>().EnableButtons();
            //PhotonNetwork.Instantiate(photonUserPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            GetComponent<PlayerManager>().HeadHelper.GetComponent<SyncPos>()
                .photonView.RPC("SpawnHead", newPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
            GetComponent<PlayerManager>().BoardHelper.GetComponent<SyncPos>()
                .photonView.RPC("SpawnHead", newPlayer, PhotonNetwork.LocalPlayer.ActorNumber);
        }
        public override void OnLeftRoom()
        {
            Debug.Log("Left Room :(");
        }
        [PunRPC]
        public void RPC_Test(string message, int[] textureSize, byte[] texture, byte[][] models)
        {
            Debug.Log(message);
            Texture2D tex = new Texture2D(textureSize[0], textureSize[1]);
            tex.LoadRawTextureData(texture);
            Debug.Log(tex.width);
            // print all models
            for (int i = 0; i < models.Length; i++)
            {
                Debug.Log("Top level: " + models[i].Length);
                for (int j = 0; j < models[i].Length; j++)
                {
                    Debug.Log(models[i][j]);
                }
            }
        }

        [PunRPC]
        public void MasterSend()
        {
            Texture2D texture = new Texture2D(3, 3);
            Debug.Log("Master: " + texture.width);
            byte[][] bArr = new byte[][]{
                new byte[]{1,2,3},
                new byte[]{4,5,6},
                new byte[]{7,8,9}
            };
            texture.GetRawTextureData();
            this.photonView.RPC("RPC_Test", RpcTarget.All, "MasterSend", new int[] { texture.width, texture.height }, texture.GetRawTextureData(), bArr);
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