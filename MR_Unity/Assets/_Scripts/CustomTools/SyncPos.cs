using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using WiapMR.PUN;

public class SyncPos : MonoBehaviourPunCallbacks
{
    public GameObject otherToSync;
    public int playerID = -2;
    
    private void Start()
    {
        if (!photonView.IsMine)
        {
            photonView.RPC("SpreadName", RpcTarget.Others, "");
        }
    }
    private void Update()
    {
        if (otherToSync != null)
        {
            transform.localPosition = otherToSync.transform.position;
            transform.localRotation = otherToSync.transform.rotation;
            transform.localScale = otherToSync.transform.localScale;
        }
    }

    [PunRPC]
    public void SpreadName(string name)
    {
        if (this.photonView.IsMine)
        {
            photonView.RPC("SpreadName", RpcTarget.Others, gameObject.name);
        }
        else
        {
            if (name != "")
            {
                gameObject.name = name;
            }
        }
    }

    [PunRPC]
    public void SpawnHead(int playerID)
    {
        GameObject _player = GameObject.Find("Player" + playerID);
        if (_player == null)
            _player = new GameObject("Player" + playerID);
        transform.parent = _player.transform;

        if (gameObject.name.StartsWith("HeadPosHelper"))
        {
            PlayerManager pm = GameObject.FindObjectOfType<PlayerManager>();
            GameObject _head = Instantiate(pm.PlayerHeadPrefab, Vector3.zero, Quaternion.identity, _player.transform);
            _head.transform.parent = _player.transform;
            _head.GetComponent<HeadSync>().initTracking(gameObject, GameObject.Find("BoardPosHelper" + playerID), false);
        }
    }

}
