using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

namespace WiapMR.PUN
{
    public class ButtonHelper : MonoBehaviour
    {
        public GameObject spawnableCube;
        public GameObject spawnableSphere;
        public GameObject spawnableBoard;
        public GameObject spawnableFloor;
        public float spawnDistance;
        public TMPro.TextMeshPro gravitySliderLabel;
        private GameObject gravitySlider;

        public GameObject buttonCollection;
        public GameObject connectingText;
        private List<GameObject> cubeList;

        private GameObject floor;
        PhotonView photonView;
        [HideInInspector] public GameObject FarCubeInstance;
        [HideInInspector] public GameObject NearCubeInstance;
        private GameObject gameboard;

        private Vector3 startPos;
        // Start is called before the first frame update
        void Start()
        {
            startPos = this.transform.position;
            Physics.gravity = new Vector3(0, -1f, 0);
            cubeList = new List<GameObject>();
            photonView = gameObject.GetComponent<PhotonView>();
            gravitySlider = GameObject.Find("Gravity_Slider");
        }

        public void EnableButtons()
        {
            buttonCollection.SetActive(true);
            connectingText.SetActive(false);
        }

        public void GravityUpdate(SliderEventData eventData)
        {
            photonView.RPC("UpdateSlider", RpcTarget.Others, eventData.NewValue);
            float gravity = eventData.NewValue * 5;
            Physics.gravity = new Vector3(0, -gravity, 0);
            gravitySliderLabel.text = "Gravity: " + gravity;
        }

        [PunRPC]
        public void UpdateSlider(float value)
        {
            gravitySlider.GetComponentInChildren<PinchSlider>().SliderValue = value;
        }

        public void ToggleGravity()
        {
            //call ToggleGravity on all cubes and then update gravity on all cubes
            foreach (GameObject cube in cubeList)
            {
                cube.GetComponent<PhotonView>().RPC("ToggleGravity", RpcTarget.All);
                cube.GetComponent<PhotonView>().RPC("UpdateGravity", RpcTarget.All);
            }
        }

        public void ResetTransform()
        {
            //Build temporary list of owned cubes
            List<GameObject> myCubes = new List<GameObject>();
            foreach (GameObject cube in cubeList)
            {
                if (cube.GetComponent<PhotonView>().IsMine)
                {
                    myCubes.Add(cube);
                }
            }
            //Reset all owned cubes
            Transform baseTransform = Camera.main.transform;
            for (int i = 0; i < myCubes.Count; i++)
            {
                myCubes[i].GetComponent<CubeController>().ResetTransform(i + 1, myCubes.Count, baseTransform);
            }
        }

        public void cycleColor()
        {
            if (cubeList.Count > 0)
            {
                foreach (GameObject cube in cubeList)
                {
                    cube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
                }
            }
        }

        public void toggleFloor()
        {
            if (floor == null)
            {
                floor = PhotonNetwork.Instantiate(spawnableFloor.name, new Vector3(0, -1, 0), Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Destroy(floor);
            }
        }

        public void spawnCube()
        {
            SpawnEntity(spawnableCube);
        }

        public void SpawnEntity(GameObject objectToSpawn)
        {

            Transform basePos = Camera.main.transform;
            var entity = PhotonNetwork.Instantiate(objectToSpawn.name, basePos.position + basePos.forward * spawnDistance, basePos.rotation, 0);
            entity.GetComponent<Renderer>().material.color = Color.gray;
            Debug.Log("Before adding to list" + cubeList.Count);
            cubeList.Add(entity);
            //logical AND on both ManipulationHandFlagTypes
            // cube.GetComponent<ObjectManipulator>().ManipulationType = ManipulationHandFlags.OneHanded | ManipulationHandFlags.TwoHanded;
            entity.GetComponent<ObjectManipulator>().enabled = true;
            Debug.Log("pv: " + photonView + "cubeID: " + entity.GetPhotonView().ViewID);
            photonView.RPC("setCubeParent", RpcTarget.All, entity.GetPhotonView().ViewID, gameboard.GetPhotonView().ViewID);
        }

        [PunRPC]
        public void setCubeParent(int cubeID, int parentID)
        {
            PhotonView.Find(cubeID).transform.SetParent(PhotonView.Find(parentID).transform);
        }

        public void DestroyCubes()
        {
            Debug.Log("DestroyCubes called...");
            Debug.Log("CubeList: " + cubeList.Count);
            //Iterate reverse since some cubes will be destroyed which changes the list
            for (int i = cubeList.Count - 1; i >= 0; i--)
            {
                Debug.Log("Destroying cube: " + cubeList[i]);
                if (cubeList[i].GetComponent<PhotonView>().IsMine)
                {
                    Debug.Log("Actually destroying...");

                    PhotonNetwork.Destroy(cubeList[i]);
                    cubeList.Remove(cubeList[i]);
                }
            }
        }

        public void spawnSphere()
        {
            SpawnEntity(spawnableSphere);
        }

        public void SpawnGameBoard()
        {
            if (gameboard == null)
            {
                gameboard = PhotonNetwork.Instantiate(spawnableBoard.name, new Vector3(0, -1, 0), Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Destroy(gameboard);
            }
        }
    }
}