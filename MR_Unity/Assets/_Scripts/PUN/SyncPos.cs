using UnityEngine;
using Photon.Pun;
using WiapMR.PlayerScripts;

namespace WiapMR.PUN
{
    /// <summary>
    /// Modular script to sync position of an object for the helper objects.
    /// </summary>
    public class SyncPos : MonoBehaviourPunCallbacks
    {
        public GameObject OtherToSync;

        private void Start()
        {
            if (!photonView.IsMine)
            {
                photonView.RPC("SpreadName", RpcTarget.Others, "");
            }
        }
        private void Update()
        {
            if (OtherToSync != null)
            {
                transform.localPosition = OtherToSync.transform.position;
                transform.localRotation = OtherToSync.transform.rotation;
                transform.localScale = OtherToSync.transform.localScale;
            }
        }

        /// <summary>
        /// Sets the correct name to the gameobject.
        /// </summary>
        /// <param name="name">Name to be given</param>
        [PunRPC]
        public void SpreadName(string name)
        {
            if (this.photonView.IsMine)
            {
                photonView.RPC("SpreadName", RpcTarget.Others, gameObject.name);
            }
            else
            {
                if (name != "")
                {
                    gameObject.name = name;
                }
            }
        }

        /// <summary>
        /// Spawns a new Head object to display other players head.
        /// </summary>
        /// <param name="playerID">ID of Player to be displayed</param>
        [PunRPC]
        public void SpawnHead(int playerID)
        {
            GameObject _player = GameObject.Find("Player" + playerID);
            if (_player == null)
                _player = new GameObject("Player" + playerID);
            transform.parent = _player.transform;

            if (gameObject.name.StartsWith("HeadPosHelper"))
            {
                PlayerManager pm = GameObject.FindObjectOfType<PlayerManager>();
                GameObject _head = Instantiate(pm.PlayerHeadPrefab, Vector3.zero, Quaternion.identity, _player.transform);
                _head.transform.parent = _player.transform;
                _head.GetComponent<HeadSync>().InitTracking(gameObject, GameObject.Find("BoardPosHelper" + playerID), false);
            }
        }

    }
}