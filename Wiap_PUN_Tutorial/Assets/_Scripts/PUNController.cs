using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PUNController : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    private void Start()
    {
        Debug.Log("Attempting to connect to Master Server...");
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }
    public override void OnJoinedRoom()
    {
        // called when we join a room
        Debug.Log("Joined room");
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Player entered room: " + newPlayer.NickName);
        // called when a player enters the room
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Left room");
        // called when we leave the room
    }
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        Debug.Log("Ownership request: " + targetView.ViewID + " from " + requestingPlayer.NickName);
        if (targetView.IsMine)
        {
            targetView.TransferOwnership(requestingPlayer);
        }
    }
    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (previousOwner == PhotonNetwork.LocalPlayer)
        {
            targetView.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else if (targetView.IsMine)
        {
            targetView.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
        }
        Debug.Log("Ownership transfered: " + targetView.ViewID + " from " + previousOwner.NickName);
    }
    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log("Ownership transfer failed: " + targetView.ViewID + " from " + senderOfFailedRequest.NickName);
    }

    public GameObject playerPrefab;
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 2, Quaternion.identity);
            }
        }
    }
}
