using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WiapMR.PUN
{
    public class HeadSync : MonoBehaviour
    {
        Camera cam;
        PhotonView pv;
        void Start()
        {
            Debug.Log("Head alive!");
            cam = Camera.main;
            pv = GetComponent<PhotonView>();
            Debug.Log("Is in room: " + PhotonNetwork.InRoom + " |Room: " +  PhotonNetwork.CurrentRoom);
        }

        // Update is called once per frame
        void Update()
        {
            if (pv.IsMine)
            {
                // Debug.Log("Head is mine!");
                transform.localPosition = cam.transform.position;
                transform.localRotation = cam.transform.rotation;
            }
        }

        private void OnDestroy()
        {
            // Debug.Log("Head destroyed!");
            Debug.Log("Is in room: " + PhotonNetwork.InRoom + " |Room: " +  PhotonNetwork.CurrentRoom);
        }
    }
}