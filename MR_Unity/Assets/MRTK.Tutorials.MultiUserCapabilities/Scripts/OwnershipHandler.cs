using System;
using Microsoft.MixedReality.Toolkit.Input;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{
    [RequireComponent(typeof(PhotonView), typeof(GenericNetSync))]
    public class OwnershipHandler : MonoBehaviourPun, IPunOwnershipCallbacks, IMixedRealityInputHandler
    {
        public void OnInputDown(InputEventData eventData)
        {
            photonView.RequestOwnership();
            Debug.Log("Requested ownership");
        }

        public void OnInputUp(InputEventData eventData)
        {
        }

        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            targetView.TransferOwnership(requestingPlayer);
            Debug.Log("Ownership requested to " + requestingPlayer.NickName);
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player previousOwner)
        {
        }

        private void TransferControl(Player idPlayer)
        {
            if (photonView.IsMine)
            {
                photonView.TransferOwnership(idPlayer);
                Debug.Log("Ownership transferred to " + idPlayer.NickName);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (photonView != null)
            {
                photonView.RequestOwnership();
                Debug.Log("Requested ownership on trigger enter");
            }

        }

        private void OnTriggerExit(Collider other)
        {
        }

        public void RequestOwnership()
        {
            photonView.RequestOwnership();
        }
    }
}
