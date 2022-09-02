using UnityEngine;
using Photon.Pun;
using WiapMR.PlayerScripts;

namespace WiapMR.PUN
{
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