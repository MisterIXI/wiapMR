using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

namespace WiapMR.GameScripts
{
    public class PlaceableObject : MonoBehaviourPun, IMixedRealityInputHandler
    {
        private GameObject _board;
        private Collider _ownCollider;
        private bool _snapped;
        private GameObject _snappedTo;
        private SnapPoint _potentialSnapPoint;
        public bool IsGrabbing = false;


        void OnTriggerEnter(Collider collider)
        {
            // Debug.Log("Collision with SnapPoint (Enter): " + collider.gameObject.tag + " | " + IsGrabbing);
            if (collider.gameObject.tag == "SnapPoint" && IsGrabbing)
            {
                // Debug.Log("Collision with SnapPoint (ACTUALLY_ENTER)");
                if (_potentialSnapPoint != null)
                {
                    _potentialSnapPoint.UnhighlightHologram();
                }
                _potentialSnapPoint = collider.gameObject.GetComponent<SnapPoint>();
                _potentialSnapPoint.HighlightHologram();
            }
        }


        void OnTriggerExit(Collider collider)
        {
            // Debug.Log("Collision with SnapPoint (Leave)");
            if (collider.gameObject.tag == "SnapPoint" && IsGrabbing)
            {
                if (_potentialSnapPoint == collider.gameObject.GetComponent<SnapPoint>())
                {
                    _potentialSnapPoint.UnhighlightHologram();
                    _potentialSnapPoint = null;
                }
            }
        }

        public bool IsSnapped()
        {
            return _snapped;
        }

        public void OnInputDown(InputEventData eventData)
        {
            if (photonView.IsMine)
            {

                if (IsSnapped())
                {
                    photonView.RPC("UnSnap", RpcTarget.All);
                }
                SnapPoint.HolographicPreviewAll(gameObject);
                IsGrabbing = true;
            }
            else
            {
                photonView.RequestOwnership();
            }
        }
        public void OnInputUp(InputEventData eventData)
        {
            if (photonView.IsMine)
            {
                if (!_snapped && _potentialSnapPoint != null)
                {
                    photonView.RPC("SnapTo", RpcTarget.All);
                    // transform.position = potentialSnapPoint.transform.position;
                }
                SnapPoint.StopHolographicPreviewAll();
                IsGrabbing = false;
            }
        }

        [PunRPC]
        public void UnSnap()
        {
            this._snapped = false;

        }

        [PunRPC]
        public void SnapTo()
        {
            _snapped = true;

        }

        void Start()
        {
            this._snapped = false;
            _board = GameObject.FindGameObjectWithTag("GameBoard");
            if (photonView.IsMine)
            {
                GetComponent<ObjectManipulator>().enabled = true;
            }
        }

    }
}