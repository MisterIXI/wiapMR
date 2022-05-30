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
            cam = Camera.main;
            pv = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update()
        {
            if (pv.IsMine)
            {
                transform.localPosition = cam.transform.position;
                transform.localRotation = cam.transform.rotation;
            }
        }
    }
}