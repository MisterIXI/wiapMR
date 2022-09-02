using UnityEngine;
using Photon.Pun;
using WiapMR.GameScripts;
using WiapMR.PUN;

namespace WiapMR.PlayerScripts
{
    /// <summary>
    /// This class is used to sync the head position of the player depending on the board location & rotation.
    /// The calculation is sophisticated since it needs to take lokal board and remote board position & rotation into account.
    /// </summary>
    public class HeadSync : MonoBehaviour
    {
        private Camera _cam;
        private PhotonView _pv;
        private GameObject _head;
        private GameObject _headHelper;
        private GameObject _boardHelper;
        private GameObject _board;
        private bool _isInitialized = false;
        private bool _isPlayer = false;

        public void InitTracking(GameObject headHelper, GameObject boardHelper, bool isPlayer)
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                this._isPlayer = isPlayer;
                this._headHelper = headHelper;
                this._boardHelper = boardHelper;
                // Debug.Log("Head alive! | " + (isPlayer ? "Player" : "Other"));
                _cam = Camera.main;
                _pv = _headHelper.GetComponent<PhotonView>();
                GameImporter gi = GameObject.FindObjectOfType<GameImporter>();
                // if (gi != null)
                //     gi.CheckForPlayers(gi.GameRoot);
                Debug.Log("Is in room: " + PhotonNetwork.InRoom + " |Room: " + PhotonNetwork.CurrentRoom);
                if (_pv.IsMine)
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
            if (_isInitialized)
            {

                if (_isPlayer)
                {
                    if (_pv != null && _pv.IsMine)
                    {
                        transform.localPosition = _cam.transform.position;
                        transform.localRotation = _cam.transform.rotation;
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
                        if (_headHelper != null)
                        {
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