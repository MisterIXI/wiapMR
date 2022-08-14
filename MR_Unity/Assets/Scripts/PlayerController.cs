using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerController : MonoBehaviour
{
    private PhotonView _photonView;
    void Start()
    {
        if (!_photonView.IsMine)
        {
            transform.parent = GameObject.FindObjectOfType<GameController>().gameObject.transform;
        }
    }
}
