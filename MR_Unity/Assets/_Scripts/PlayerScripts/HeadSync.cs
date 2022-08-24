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
        public GameObject parent;
        public GameObject helper;
        private GameObject head;
        private bool isInitialized = false;
        void Start()
        {
            Debug.Log("Head alive!");
            cam = Camera.main;
            pv = GetComponent<PhotonView>();
            GameImporter gi = GameObject.FindObjectOfType<GameImporter>();
            if (gi != null)
                gi.CheckForPlayers(gi.GameRoot);
            Debug.Log("Is in room: " + PhotonNetwork.InRoom + " |Room: " + PhotonNetwork.CurrentRoom);
            if (pv.IsMine)
                GetComponent<MeshRenderer>().enabled = false;
            if (pv.IsMine)
            {
                helper = new GameObject("Helper");
                helper.transform.parent = parent.transform;
            }
        }
        public void initTracking(GameObject parent)
        {
            if (!isInitialized)
            {
                this.parent = parent;
                isInitialized = true;
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (pv.IsMine)
            {
                // Debug.Log("Head is mine!");
                if (parent == null)
                {
                    transform.localPosition = cam.transform.position;
                    transform.localRotation = cam.transform.rotation;
                }
                else
                {
                    transform.localPosition = cam.transform.position - parent.transform.position;
                    // invert parent transform rotation
                    // Quaternion rot = Quaternion.Inverse(parent.transform.rotation);
                    // transform.localRotation = cam.transform.rotation * Quaternion.Inverse(parent.transform.rotation);
                }
            }
        }

        private void OnDestroy()
        {
            // Debug.Log("Head destroyed!");
            Debug.Log("Is in room: " + PhotonNetwork.InRoom + " |Room: " + PhotonNetwork.CurrentRoom);
        }
    }
}