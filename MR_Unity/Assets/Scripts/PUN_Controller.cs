using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace WiapMR.PUN
{
    public class PUN_Controller : MonoBehaviourPunCallbacks
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
            //PhotonNetwork.Instantiate(photonUserPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        }

    }
}