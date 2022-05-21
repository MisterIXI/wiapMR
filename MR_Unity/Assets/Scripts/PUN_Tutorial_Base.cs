using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PUN_Tutorial_Base : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.SendRate = 1;
    }
    bool isSpawned = false;
    public GameObject photonUserPrefab;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire3"))
        {
            if(!isSpawned)
            {
                
                PhotonNetwork.Instantiate(photonUserPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
                //isSpawned = true;
            }
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        //PhotonNetwork.Instantiate(photonUserPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
