using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class DeathCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        PhotonNetwork.Destroy(other.gameObject);
    }
}
