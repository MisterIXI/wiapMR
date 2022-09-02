using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.Input;
using Photon.Realtime;

namespace CubeScene.PUN
{
    [RequireComponent(typeof(PhotonView))]
    public class CubeController : MonoBehaviourPun, IMixedRealityInputHandler
    {
        public bool isGrabbing { get; private set; } = false;
        PhotonView pv;
        Rigidbody rb;
        bool gravityEnabled;
        private GameObject floor;

        void Start()
        {
            pv = GetComponent<PhotonView>();
            rb = GetComponent<Rigidbody>();

        }
        [PunRPC]
        public void ToggleGravity()
        {
            gravityEnabled = !gravityEnabled;
        }

        [PunRPC]
        public void UpdateGravity()
        {
            if (pv.IsMine)
            {
                rb.useGravity = gravityEnabled;
                rb.isKinematic = !gravityEnabled;
            }
        }

        [PunRPC]
        public void DestroyThis()
        {
            if (pv.IsMine)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Debug.Log("Couldn't delete since it's not mine");
            }
        }
        //resets the cubes to theirs originalPosition and size
        public void ResetTransform(int position, int totalCount, Transform basePosition)
        {
            //ownership should be already checked earlier, but as a security this is done again
            if (pv.IsMine)
            {
                transform.position = basePosition.position + basePosition.forward * 1 - basePosition.right * 0.5f * (totalCount / 2 - position);
                transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                transform.rotation = basePosition.rotation;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
        public void OnInputDown(InputEventData eventData)
        {
            if (!photonView.IsMine)
            {
                photonView.RequestOwnership();
                Debug.Log("Requested ownership");
            }
            else
            {
                isGrabbing = true;
            }
        }
        public void OnInputUp(InputEventData eventData)
        {
            if(photonView.IsMine)
            {
                isGrabbing = false;
            }
        }


    }
}