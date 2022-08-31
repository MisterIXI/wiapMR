using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace WiapMR.PUN
{
    public class HeadSync : MonoBehaviour
    {
        Camera cam;
        public PhotonView pv;
        private GameObject _head;
        private GameObject _headHelper;
        private GameObject _boardHelper;
        private GameObject _board;
        private bool isInitialized = false;
        public bool isPlayer = false;
        void Start()
        {

        }
        public void initTracking(GameObject headHelper, GameObject boardHelper, bool isPlayer)
        {
            if (!isInitialized)
            {
                isInitialized = true;
                this.isPlayer = isPlayer;
                this._headHelper = headHelper;
                this._boardHelper = boardHelper;
                // Debug.Log("Head alive! | " + (isPlayer ? "Player" : "Other"));
                cam = Camera.main;
                pv = _headHelper.GetComponent<PhotonView>();
                GameImporter gi = GameObject.FindObjectOfType<GameImporter>();
                // if (gi != null)
                //     gi.CheckForPlayers(gi.GameRoot);
                Debug.Log("Is in room: " + PhotonNetwork.InRoom + " |Room: " + PhotonNetwork.CurrentRoom);
                if (pv.IsMine)
                {
                    var renderers = GetComponentsInChildren<MeshRenderer>();
                    foreach (var r in renderers)
                    {
                        r.enabled = false;
                    }
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (isInitialized)
            {

                if (isPlayer)
                {
                    if (pv != null && pv.IsMine)
                    {
                        transform.localPosition = cam.transform.position;
                        transform.localRotation = cam.transform.rotation;
                    }
                }
                else
                {
                    if (_board != null && _boardHelper != null)
                    {
                        Vector3 newPos = _headHelper.transform.position - _boardHelper.transform.position;
                        newPos = Quaternion.Inverse(_boardHelper.transform.rotation) * newPos;
                        newPos = _board.transform.rotation * newPos;
                        newPos += _board.transform.position;
                        transform.position = newPos;
                        Quaternion newRot = _headHelper.transform.rotation;
                        newRot = Quaternion.Inverse(_boardHelper.transform.rotation) * newRot;
                        newRot = _board.transform.rotation * newRot;
                        transform.rotation = newRot;
                    }
                    else
                    {
                        if (_board == null)
                            _board = GameObject.FindObjectOfType<GameImporter>().GameBoard;
                        if (_boardHelper == null)
                        {
                            var children = transform.parent.gameObject.GetComponentsInChildren<SyncPos>();
                            foreach (var c in children)
                            {
                                if (c.gameObject.name.StartsWith("BoardPosHelper"))
                                {
                                    _boardHelper = c.gameObject;
                                }
                            }
                        }
                        if(_headHelper != null){
                            transform.position = _headHelper.transform.position;
                            transform.rotation = _headHelper.transform.rotation;
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            // Debug.Log("Head destroyed!");
            // Debug.Log("Is in room: " + PhotonNetwork.InRoom + " |Room: " + PhotonNetwork.CurrentRoom);
        }
    }
}