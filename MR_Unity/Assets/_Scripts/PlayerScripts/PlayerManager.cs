using UnityEngine;
using Photon.Pun;
using WiapMR.PUN;
using WiapMR.GUI;

namespace WiapMR.PlayerScripts
{
    public class PlayerManager : MonoBehaviour
    {
        public GameObject PlayerHeadPrefab;
        public GameObject PlayerHandPrefab;

        private GameObject _player;
        private GameObject _head;
        public GameObject HeadHelper;
        public GameObject BoardHelper;

        public void Initialize()
        {
            _player = new GameObject();
            _player.name = "Player" + PhotonNetwork.LocalPlayer.ActorNumber;

            // spawn trackers
            int playerID = PhotonNetwork.LocalPlayer.ActorNumber;
            HeadHelper = PhotonNetwork.Instantiate("HeadPosHelper", Vector3.zero, Quaternion.identity, 0);
            HeadHelper.name = "HeadPosHelper" + playerID;
            BoardHelper = PhotonNetwork.Instantiate("BoardPosHelper", Vector3.zero, Quaternion.identity, 0);
            BoardHelper.name = "BoardPosHelper" + playerID;
            HeadHelper.transform.parent = _player.transform;
            BoardHelper.transform.parent = _player.transform;

            // spawn player head
            Transform camera_transform = Camera.main.transform;
            Debug.Log("Camera pos: " + camera_transform.position);
            _head = Instantiate(PlayerHeadPrefab, camera_transform.position, camera_transform.rotation, _player.transform);
            _head.GetComponent<HeadSync>().InitTracking(HeadHelper, BoardHelper, true);
            _head.transform.parent = _player.transform;

            // set object to track to headTracker
            HeadHelper.GetComponent<SyncPos>().OtherToSync = _head;
            // boardTracker object needs to be set after GameImporter is done
        }

        [PunRPC]
        public void DisableStartMenu()
        {
            GameObject.FindObjectOfType<StartPlate>().gameObject.SetActive(false);
        }
    }
}