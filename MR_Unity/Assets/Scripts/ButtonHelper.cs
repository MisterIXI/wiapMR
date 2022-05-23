using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.UI;

namespace WiapMR.PUN
{
    public class ButtonHelper : MonoBehaviour
    {
        public GameObject spawnableCube;
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
            photonView = GameObject.Find("MP_Controller").GetComponent<PhotonView>();
            Debug.Log("Components: ");
            foreach (Component c in GameObject.Find("MP_Controller").GetComponents(typeof(Component)))
            {
                Debug.Log("Component: " + c.GetType().Name);

            }
            Debug.Log("PhotonView: " + photonView + " EventData: " + eventData.NewValue + " GravityUpdate" + "Connected: " + PhotonNetwork.IsConnected); photonView.RPC("UpdateSlider", RpcTarget.Others, eventData.NewValue);
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
            Transform basePos = Camera.main.transform;
            var cube = PhotonNetwork.Instantiate(spawnableCube.name, basePos.position + basePos.forward * spawnDistance, basePos.rotation, 0);
            cube.GetComponent<Renderer>().material.color = Color.gray;
            Debug.Log("Before adding to list" + cubeList.Count);
            cubeList.Add(cube);
            Debug.Log("After adding to list" + cubeList.Count);
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
    }
}