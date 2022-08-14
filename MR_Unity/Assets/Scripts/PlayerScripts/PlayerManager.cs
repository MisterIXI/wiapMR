using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    public GameObject PlayerHeadPrefab;
    public GameObject PlayerHandPrefab;

    public GameObject _player;
    public  GameObject _head;


    public void Initialize()
    {
        _player = new GameObject();
        _player.name = "Player" + PhotonNetwork.LocalPlayer.ActorNumber;
        SpawnHead(_player);
    }

    public void SpawnHead(GameObject parent)
    {
        Transform camera_transform = Camera.main.transform;
        Debug.Log("Camera pos: " + camera_transform.position);
        _head = PhotonNetwork.Instantiate(PlayerHeadPrefab.name, camera_transform.position, camera_transform.rotation);
        // _head = Instantiate(PlayerHeadPrefab, camera_transform.position, camera_transform.rotation);
        // head.transform.parent = parent.transform;
        Debug.Log("Spawned head" + _head.name);
    }
}
