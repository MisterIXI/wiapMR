using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PUN_Tutorial_Ownership : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
{
    public Material redMaterial;
    public Material blueMaterial;

    void Start()
    {
    }
    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
   {
       
        Debug.Log("Ownership Requested on " + targetView.gameObject + " by " + requestingPlayer);
        if (targetView.IsMine)
        {
            targetView.gameObject.GetComponent<MeshRenderer>().material = blueMaterial;
            targetView.TransferOwnership(requestingPlayer);
        }
   }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        //targetView.gameObject.GetComponent<MeshRenderer>().material = redMaterial;
        Debug.Log("Ownership Transfered from " + previousOwner + " to " + targetView.Owner);
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        Debug.Log("Ownership Transfer Failed");
    }

    public void spawnColor(GameObject toChange)
    {
        toChange.GetComponent<MeshRenderer>().material = redMaterial;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
